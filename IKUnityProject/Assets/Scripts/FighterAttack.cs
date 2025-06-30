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
    
    private StateManager _stateManager;

    private void Awake()
    {
        _stateManager = GetComponent<StateManager>();
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
        var clip = attackData.Clip;
        var limbRigController = attackData.LimbRig;
        var curveIn = attackData.CurveIn;
        var curveOut = attackData.CurveOut;
        
        OnStartAttack?.Invoke();
        var clipDuration = clip.length;
        var actionConnection = clip.events[0].time;
        Debug.Log($"Clip Duration: {clipDuration}, Action Connection: {actionConnection}");
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
        OnEndAttack?.Invoke();
    }
}