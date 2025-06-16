using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FighterAttack : MonoBehaviour
{
    public Action OnStartAttack;
    public Action OnStartRecover;
    public Action OnEndAttack;
    
    [SerializeField] private AttackData _lPunchData;
    [SerializeField] private AttackData _mPunchData;

    private StateManager _stateManager;

    private void OnLAttack(InputValue value)
    {
        StartCoroutine(ActionCoroutine(_lPunchData));
    }
    
    private void OnMAttack(InputValue value)
    {
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
        while (timer < actionConnection)
        {
            limbRigController.SetRigWeight(curveIn.Evaluate(timer / actionConnection));
            timer += Time.deltaTime;
            yield return null;
        }
        
        limbRigController.SetRigWeight(1);
        OnStartRecover?.Invoke();
        
        var remainingTime = clipDuration - actionConnection;
        
        while (timer < remainingTime)
        {
            limbRigController.SetRigWeight(curveOut.Evaluate((timer / remainingTime)));
            timer += Time.deltaTime;
            yield return null;
        }

        limbRigController.SetRigWeight(0);
        OnEndAttack?.Invoke();
    }
}