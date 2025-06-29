using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FighterMovement : MonoBehaviour
{
    [Header("Movement")]
    public Action OnMoveStart;
    public Action OnMoveStop;
    public float MoveSpeed = 5f;
    private float _moveInput;
    private bool _canMove = true;
    private bool _flipped;
    
    [Header("Jump")]
    public Action OnJumpStart;
    public Action OnJumpLand;
    public float JumpForce = 5f;
    public float GroundDistance = 0.2f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private bool _isJumping;
    
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

    private void OnJump(InputValue value)
    {
        if (_isJumping)
            return;

        _isJumping = true;
        OnJumpStart?.Invoke();
        _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

    private void Update()
    {
        var thisFrameIsGrounded = Physics.Raycast(_groundCheck.position, -_groundCheck.up, GroundDistance, _groundMask);
        if (thisFrameIsGrounded && _isJumping && _rb.linearVelocity.y < 0)
        {
            _isJumping = false;
            OnJumpLand?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        if(!_canMove)
            return;
        
        if (_moveInput != 0)
            OnMoveStart?.Invoke();
        else
            OnMoveStop?.Invoke();

        var movement = Vector3.forward * (_moveInput * (MoveSpeed * Time.fixedDeltaTime));
        _rb.MovePosition(_rb.position + movement);
    }
}