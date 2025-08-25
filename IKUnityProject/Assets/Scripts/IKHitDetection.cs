using UnityEngine;

public abstract class IKHitDetection : MonoBehaviour
{
    [SerializeField] protected HitReact _hitReact;
    [SerializeField] protected AnimationCurve _curve;
    
    private void Awake()
    {
        _hitReact.OnHit += OnHit;
    }

    protected abstract void OnHit(HitData hitData);
}