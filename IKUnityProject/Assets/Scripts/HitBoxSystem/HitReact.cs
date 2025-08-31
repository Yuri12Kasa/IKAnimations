using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitReact : MonoBehaviour
{
    public Action<HitData> OnHit;
    public int PlayerNumber => _stateManager.PlayerNumber;
    [SerializeField] private StateManager _stateManager;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }
    
    public void EnableTrigger()
    {
        _collider.enabled = true;
    }
    
    public void DisableTrigger()
    {
        _collider.enabled = false;
    }

    public void ApplyHit(HitData hitData)
    {
        OnHit?.Invoke(hitData);
        Debug.Log($"Applied Hit Data: Direction {hitData.Direction}, Damage: {hitData.Damage}, Force: {hitData.Force}");
    }
}