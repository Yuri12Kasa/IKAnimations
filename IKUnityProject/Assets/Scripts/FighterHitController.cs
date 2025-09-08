using System;
using Health;
using UnityEngine;
using UnityEngine.InputSystem;

public class FighterHitController : MonoBehaviour
{
    public Action StartStun;
    public Action StopStun;
    public static Action<Transform> OnDeath;
    
    [SerializeField] private IKHitDetection[] _hitDetections;
    
    private HealthComponent _healthComponent;
    private RagdollController _ragdollController;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _healthComponent =  GetComponent<HealthComponent>();
        
        _ragdollController = GetComponent<RagdollController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        foreach (var hitDetection in _hitDetections)
        {
            hitDetection.StartStun += () => StartStun?.Invoke();
            hitDetection.OnTakeDamage += _healthComponent.TakeDamage;
            hitDetection.StopStun += () => StopStun?.Invoke();

            _healthComponent.OnDeath += Death;
        }
    }

    private void Death()
    {
        _playerInput.enabled = false;
        _ragdollController.EnableRagdoll();
        OnDeath?.Invoke(transform);
    }
}