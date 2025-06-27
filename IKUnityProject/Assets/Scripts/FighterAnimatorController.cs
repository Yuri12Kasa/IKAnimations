using UnityEngine;
using UnityEngine.InputSystem;

public class FighterAnimatorController : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int LAttack = Animator.StringToHash("LAttack");
    private static readonly int MAttack = Animator.StringToHash("MAttack");
    private static readonly int Jump = Animator.StringToHash("Jump");
    [SerializeField] private Animator _animator;

    private StateManager _state;
    private float _moveInput;
    private float _moveSpeed = 0.5f;
    private bool _canMove = true;
    [SerializeField] private bool _flipped;

    private void Awake()
    {
        _state = GetComponent<StateManager>();
        _state.OnStateChange += OnStateChange;
    }

    private void OnStateChange(FighterState newState)
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

    private void OnLAttack(InputValue value)
    {
        _animator.SetTrigger(LAttack);
    }

    private void OnMAttack(InputValue value)
    {
        _animator.SetTrigger(MAttack);
    }

    private void OnJump(InputValue value)
    {
        _animator.SetBool(Jump, true);
    }

    private void Update()
    {
        if (!_canMove)
            return;
        
        _animator.SetFloat(Speed, _moveSpeed);
        _flipped = Mathf.Approximately(transform.eulerAngles.y, 180);
        var minMax = _flipped ? new Vector2(1f, -1f) : new Vector2(-1f, 1f);
        _moveSpeed = Mathf.InverseLerp(minMax.x, minMax.y, _moveInput);
    }
}
