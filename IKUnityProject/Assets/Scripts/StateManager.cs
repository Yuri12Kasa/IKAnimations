using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateManager : MonoBehaviour
{
    public Action<FighterState> OnStateChange;

    [field: ReadOnly]
    [field: SerializeField]
    public FighterState State { get; private set; } = FighterState.Neutral;

    private FighterAttack _fighterAttack;

    private void Awake()
    {
        _fighterAttack = GetComponent<FighterAttack>();
        _fighterAttack.OnStartAttack += () => SetState(FighterState.Startup);
        _fighterAttack.OnStartActiveFrames += () => SetState(FighterState.Active);
        _fighterAttack.OnStartRecover += () => SetState(FighterState.Recover);
        _fighterAttack.OnEndAttack += () => SetState(FighterState.Neutral);
    }

    private void SetState(FighterState fighterState)
    {
        State = fighterState;
        OnStateChange?.Invoke(fighterState);
    }
}