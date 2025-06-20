using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FighterAnimatorController : MonoBehaviour
{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int LAttack = Animator.StringToHash("LAttack");
    private static readonly int MAttack = Animator.StringToHash("MAttack");
    [SerializeField] private Animator _animator;

    private StateManager _state;
    private float _moveSpeed = 0.5f;
    private bool _canMove = true;

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
        _moveSpeed = Mathf.InverseLerp(-1f, 1f, value.Get<float>());
    }

    private void OnLAttack(InputValue value)
    {
        _animator.SetTrigger(LAttack);
    }

    private void OnMAttack(InputValue value)
    {
        _animator.SetTrigger(MAttack);
    }

    private void Update()
    {
        if(_canMove)
            _animator.SetFloat(Speed, _moveSpeed);
    }
}
