using System;
using UnityEngine;

public abstract class IKHitDetection : MonoBehaviour
{
    public Action StartStun;
    public Action<int> OnTakeDamage;
    public Action StopStun;
    
    [SerializeField] protected HitReact _hitReact;
    [SerializeField] protected AnimationCurve _curve;
    
    protected virtual void Start()
    {
        _hitReact.OnGetHit += OnHit;
    }

    protected abstract void OnHit(HitData hitData, bool isProtected);

    protected virtual void OnDestroy()
    {
        _hitReact.OnGetHit -= OnHit;
    }
}