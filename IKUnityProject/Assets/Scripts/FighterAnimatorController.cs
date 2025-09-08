using UnityEngine;

public class FighterAnimatorController : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int LAttack = Animator.StringToHash("LAttack");
    private static readonly int MAttack = Animator.StringToHash("MAttack");
    private static readonly int Jump = Animator.StringToHash("Jumping");
    private static readonly int Celebrate = Animator.StringToHash("Celebrate");

    [SerializeField] private Animator _animator;

    private float _moveInput;
    private float _moveSpeed = 0.5f;
    
    private FighterMovement _fighterMovement;
    private FighterAttack _fighterAttack;
    private FighterGuard _fighterGuard;
    private StateManager _stateManager;

    private void Awake()
    {
        _fighterMovement = GetComponent<FighterMovement>();
        _fighterMovement.OnJumpStart += OnStartJump;
        _fighterMovement.OnJumpLand += OnLand;

        _fighterAttack = GetComponent<FighterAttack>();
        _fighterAttack.OnStartLAttack += OnStartLAttack;
        _fighterAttack.OnStartMAttack += OnStartMAttack;
        
        _stateManager = GetComponent<StateManager>();
        _stateManager.OnStateChange += OnStateChange;
    }

    private void OnStartJump()
    {
        _animator.SetBool(Jump, true);
    }

    private void OnLand()
    {
        _animator.SetBool(Jump, false);
    }

    private void OnStartLAttack()
    {
        _animator.SetTrigger(LAttack);
    }
    
    private void OnStartMAttack()
    {
        _animator.SetTrigger(MAttack);
    }

    private void Update()
    {
        if (!_fighterMovement.CanMove)
        {
            _animator.SetFloat(Speed, 0.5f);
            return;
        }
        
        var minMax = Mathf.Approximately(transform.eulerAngles.y, 180) ? 
            new Vector2(1f, -1f) : new Vector2(-1f, 1f);
        _moveSpeed = Mathf.InverseLerp(minMax.x, minMax.y, _fighterMovement.MoveInput);
        _animator.SetFloat(Speed, _moveSpeed);
    }

    private void OnStateChange(FighterState newState)
    {
        if (newState != FighterState.Celebrating)
            return;
        
        _animator.SetTrigger(Celebrate);
    }
}
