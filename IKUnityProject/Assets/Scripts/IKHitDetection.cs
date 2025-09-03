using System;
using UnityEngine;

public abstract class IKHitDetection : MonoBehaviour
{
    public Action StartStun;
    public Action StopStun;
    [SerializeField] protected HitReact _hitReact;
    [SerializeField] protected AnimationCurve _curve;
    
    private void Awake()
    {
        _hitReact.OnGetHit += OnHit;
        _hitReact.OnGetHit += _ => StartStun?.Invoke();
        _hitReact.OnGetHit += _ => StopStun?.Invoke();
    }

    protected abstract void OnHit(HitData hitData);
}