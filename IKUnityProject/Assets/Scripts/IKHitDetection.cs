using System;
using UnityEngine;

public abstract class IKHitDetection : MonoBehaviour
{
    public Action StartStun;
    public Action<int> OnTakeDamage;
    public Action StopStun;
    
    [SerializeField] protected HitReact _hitReact;
    [SerializeField] protected AnimationCurve _curve;
    
    private void Awake()
    {
        _hitReact.OnGetHit += OnHit;
    }

    protected abstract void OnHit(HitData hitData, bool isProtected);
}