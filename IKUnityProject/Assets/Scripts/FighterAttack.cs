using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FighterAttack : MonoBehaviour
{
    public Action OnStartLAttack;
    public Action OnStartMAttack;
    
    public Action OnStartAttack;
    public Action OnStartActiveFrames;
    public Action OnStartRecover;
    public Action OnEndAttack;
    
    [SerializeField] private AttackData _lPunchData;
    [SerializeField] private AttackData _mPunchData;

    [SerializeField] private HitAttack _handHitAttack;

    private AttackData _currentAttackData;
    
    private StateManager _stateManager;

    private void Awake()
    {
        _stateManager = GetComponent<StateManager>();
    }

    private void Start()
    {
        _handHitAttack.OnHit += ComputeHit;
    }

    private void OnLAttack(InputValue value)
    {
        if (_stateManager.State != FighterState.Neutral && _stateManager.State != FighterState.Moving) 
            return;
        
        StopAllCoroutines();
        OnStartLAttack?.Invoke();
        StartCoroutine(ActionCoroutine(_lPunchData));
    }
    
    private void OnMAttack(InputValue value)
    {
        if (_stateManager.State != FighterState.Neutral && _stateManager.State != FighterState.Moving) 
            return;
        
        StopAllCoroutines();
        OnStartMAttack?.Invoke();
        StartCoroutine(ActionCoroutine(_mPunchData));
    }

    private IEnumerator ActionCoroutine(AttackData attackData)
    {
        _currentAttackData = attackData;
        var clip = attackData.Clip;
        var limbRigController = attackData.LimbRig;
        var curveIn = attackData.CurveIn;
        var curveOut = attackData.CurveOut;
        
        OnStartAttack?.Invoke();
        _handHitAttack.EnableTrigger();
        var clipDuration = clip.length;
        var actionConnection = clip.events[0].time;
        //Debug.Log($"Clip Duration: {clipDuration}, Action Connection: {actionConnection}");
        var timer = 0f;
        float t;
        while (timer < actionConnection)
        {
            t = curveIn.Evaluate(timer / actionConnection);
            limbRigController.SetRigWeight(t);
            timer += Time.deltaTime;
            yield return null;
        }
        
        limbRigController.SetRigWeight(1);
        OnStartRecover?.Invoke();
        _handHitAttack.DisableTrigger();
        
        var remainingTime = clipDuration - actionConnection;
        timer = 0f;

        while (timer < remainingTime)
        {
            t = 1 - curveOut.Evaluate(timer / remainingTime);
            limbRigController.SetRigWeight(t);
            timer += Time.deltaTime;
            yield return null;
        }

        limbRigController.SetRigWeight(0);
        _currentAttackData = null;
        OnEndAttack?.Invoke();
    }

    private void ComputeHit(HitReact hitReact)
    {
        var hitData = new HitData
        {
            Direction = (hitReact.transform.position - _handHitAttack.transform.position).normalized,
            Damage = _currentAttackData.Damage,
            Force = _currentAttackData.LimbRig.Weight
        };
        Debug.Log($"Computed Hit Data: Direction {hitData.Direction}, Damage: {hitData.Damage}, Force: {hitData.Force}");
        hitReact.ApplyHit(hitData);
    }

    private void OnDestroy()
    {
        _handHitAttack.OnHit -= ComputeHit;
    }
}