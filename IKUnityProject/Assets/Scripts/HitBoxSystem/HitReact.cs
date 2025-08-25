using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HitReact : MonoBehaviour
{
    public Action OnHit;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    public void ApplyHit(HitData hitData)
    {
        OnHit?.Invoke();
        Debug.Log($"Applied Hit Data: Direction {hitData.Direction}, Damage: {hitData.Damage}, Force: {hitData.Force}");
    }
}