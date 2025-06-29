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
    public float AirMoveSpeed = 8f;
    public float MoveInput => _moveInput;
    private float _moveInput;
    public bool CanMove => _canMove;
    private bool _canMove = true;
    private bool _flipped;
    
    [Header("Jump")]
    public Action OnJumpStart;
    public Action OnJumpLand;
    public float JumpForce = 5f;
    public float GroundDistance = 0.2f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;
    public bool IsGrounded => _isGrounded;
    [SerializeField] private bool _isGrounded = true;
    [SerializeField] private bool _wasGrounded;
    
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
        if (_stateManager.State != FighterState.Neutral && _stateManager.State != FighterState.Moving)
            return;

        _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        OnJumpStart?.Invoke();
        Debug.Log("Jump");
    }

    private void Update()
    {
        _wasGrounded = _isGrounded;
        _isGrounded = Physics.CheckSphere(_groundCheck.position, GroundDistance, _groundMask);
        if (!_wasGrounded && _isGrounded)
        {
            OnJumpLand?.Invoke();
            Debug.Log("Jump Land");
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
        
        var speed = _isGrounded ? MoveSpeed : AirMoveSpeed;

        var movement = Vector3.forward * (_moveInput * (speed * Time.fixedDeltaTime));
        _rb.MovePosition(_rb.position + movement);
    }
}