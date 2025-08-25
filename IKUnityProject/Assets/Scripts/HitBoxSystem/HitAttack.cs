using System;
using UnityEngine;

public class HitAttack : MonoBehaviour
{
    public Action<HitReact> OnHit;
    private Collider _collider;
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        DisableTrigger();
    }

    public void EnableTrigger()
    {
        _collider.enabled = true;
    }
    
    public void DisableTrigger()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HitReact"))
        {
            var hitReact = other.GetComponent<HitReact>();
            OnHit?.Invoke(hitReact);
            Debug.Log($"HitReact got hit: {hitReact.name}");
        }
    }
    
    
}