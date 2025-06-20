using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FighterMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    private float _moveInput;
    private bool _canMove = true;
    private bool _flipped;
    
    private Rigidbody _rb;
    private StateManager _stateManager;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _stateManager = GetComponent<StateManager>();
        _stateManager.OnStateChange += SetCanMove;
    }

    private void SetCanMove(FighterState newState)
    {
        _canMove = newState switch
        {
            FighterState.Neutral or FighterState.Moving => true,
            FighterState.Startup => false,
            _ => _canMove
        };
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<float>();
    }

    private void FixedUpdate()
    {
        if(!_canMove)
            return;
        
        var movement = Vector3.forward * (_moveInput * (MoveSpeed * Time.fixedDeltaTime));
        _rb.MovePosition(_rb.position + movement);
    }
}