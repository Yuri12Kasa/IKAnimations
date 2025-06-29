using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public Action<FighterState> OnStateChange;

    [field: ReadOnly]
    [field: SerializeField]
    public FighterState State { get; private set; } = FighterState.Neutral;

    private FighterAttack _fighterAttack;
    private FighterMovement _fighterMovement;

    private void Awake()
    {
        _fighterAttack = GetComponent<FighterAttack>();
        _fighterAttack.OnStartAttack += () => SetState(FighterState.Startup);
        _fighterAttack.OnStartActiveFrames += () => SetState(FighterState.Active);
        _fighterAttack.OnStartRecover += () => SetState(FighterState.Recover);
        _fighterAttack.OnEndAttack += () => SetState(FighterState.Neutral);
        
        _fighterMovement = GetComponent<FighterMovement>();
        _fighterMovement.OnMoveStart += () => SetState(FighterState.Moving);
        _fighterMovement.OnMoveStop += () => SetState(FighterState.Neutral);
        _fighterMovement.OnJumpStart += () => SetState(FighterState.Jumping);
        _fighterMovement.OnJumpLand += () => SetState(FighterState.Neutral);
    }

    private void SetState(FighterState fighterState)
    {
        State = fighterState;
        OnStateChange?.Invoke(fighterState);
    }
}