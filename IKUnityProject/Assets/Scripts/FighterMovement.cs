using UnityEngine;
using UnityEngine.InputSystem;

public class FighterMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    private float _moveInput;
    private bool _canMove = true;
    
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
        _canMove = newState == FighterState.Neutral;
    }

    private void OnMove(InputValue value)
    {
        if(_canMove)
            _moveInput = value.Get<float>();
        else
            _moveInput = 0;
    }

    private void FixedUpdate()
    {
        if(!_canMove)
            return;
        
        var movement = new Vector3(_moveInput, 0, 0) * (MoveSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.position + movement);
    }
}