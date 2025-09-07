using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public int PlayerNumber;
    public Action<FighterState> OnStateChange;

    [field: ReadOnly]
    [field: SerializeField]
    public FighterState State { get; private set; } = FighterState.Neutral;

    private FighterAttack _fighterAttack;
    private FighterMovement _fighterMovement;
    private FighterGuard _fighterGuard;
    private FighterHitController _fighterHitController;

    private void Awake()
    {
        _fighterAttack = GetComponent<FighterAttack>();
        _fighterAttack.OnStartAttack += () => SetState(FighterState.Startup);
        _fighterAttack.OnStartActiveFrames += () => SetState(FighterState.Active);
        _fighterAttack.OnStartRecover += () => SetState(FighterState.Recover);
        _fighterAttack.OnEndAttack += () => SetState(FighterState.Neutral);
        
        _fighterMovement = GetComponent<FighterMovement>();
        _fighterMovement.OnMoveStart += OnMoveStart;
        _fighterMovement.OnMoveStop += OnMoveStop;
        
        _fighterMovement.OnJumpStart += () => SetState(FighterState.Jumping);
        _fighterMovement.OnJumpLand += () => SetState(FighterState.Neutral);
        
        _fighterGuard = GetComponent<FighterGuard>();
        _fighterGuard.OnStartBlocking += OnStartBlocking;
        _fighterGuard.OnStopBlocking += OnStopBlocking;
        
        _fighterHitController = GetComponent<FighterHitController>();
        _fighterHitController.StartStun += () => SetState(FighterState.Stun);
        _fighterHitController.StopStun += () => SetState(FighterState.Neutral);
    }

    private void SetState(FighterState fighterState)
    {
        Debug.Log($"New fighter state {fighterState}");
        State = fighterState;
        OnStateChange?.Invoke(fighterState);
    }

    private void OnMoveStart()
    {
        if (State != FighterState.Neutral)
            return;
        State = FighterState.Moving;
    }
    
    private void OnMoveStop()
    {
        if (State != FighterState.Moving)
            return;
        State = FighterState.Neutral;
    }

    private void OnStartBlocking()
    {
        SetState(FighterState.Blocking);
    }

    private void OnStopBlocking()
    {
        SetState(FighterState.Neutral);
    }
    

    private void OnDestroy()
    {
        _fighterMovement.OnMoveStart += OnMoveStart;
        _fighterMovement.OnMoveStop += OnMoveStop;
    }
}