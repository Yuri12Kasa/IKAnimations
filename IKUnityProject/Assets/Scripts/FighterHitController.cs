using System;
using Health;
using UnityEngine;

public class FighterHitController : MonoBehaviour
{
    public Action StartStun;
    public Action StopStun;
    
    [SerializeField] private IKHitDetection[] _hitDetections;
    
    private HealthComponent _healthComponent;

    private void Awake()
    {
        _healthComponent =  GetComponent<HealthComponent>();
    }

    private void Start()
    {
        foreach (var hitDetection in _hitDetections)
        {
            hitDetection.StartStun += () => StartStun?.Invoke();
            hitDetection.OnTakeDamage += _healthComponent.TakeDamage;
            hitDetection.StopStun += () => StopStun?.Invoke();
        }
    }
}