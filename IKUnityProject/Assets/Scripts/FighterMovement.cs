using UnityEngine;
using UnityEngine.InputSystem;

public class FighterMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    private float _moveInput;
    [SerializeField] private bool _canMove = true;
    
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
        
        var movement = new Vector3(_moveInput, 0, 0) * (MoveSpeed * Time.fixedDeltaTime);
        _rb.MovePosition(_rb.position + movement);
    }
}