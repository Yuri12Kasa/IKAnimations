using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FighterGuard : MonoBehaviour
{
    public Action OnStartBlocking;
    public Action OnStopBlocking;

    [SerializeField] private float _blockStartTime = 0.2f;
    
    [SerializeField] private LimbRigController[] _limbRigControllers = new LimbRigController[2];

    [SerializeField] private Transform[] _blockHighPoints = new Transform[2];
    [SerializeField] private Transform[] _blockLowPoints = new Transform[2];
    [SerializeField] private Transform[] _blockTargets = new Transform[2];

    private bool _isBlocking;
    
    private PlayerInput _playerInput;
    private InputAction _blockAction;
    
    private Transform _defaultTarget;
    private StateManager _stateManager;

    private void Awake()
    {
        for (var i = 0; i < _limbRigControllers.Length; i++)
        {
            _blockTargets[i].position = _blockHighPoints[i].position;
        }
        
        _playerInput = GetComponent<PlayerInput>();
        _blockAction = _playerInput.actions.FindAction("Block");
        _blockAction.performed += StartBlocking;
        _blockAction.canceled += StopBlocking;
        
        _defaultTarget = GetComponent<TargetController>().Aim;
        _stateManager = GetComponentInParent<StateManager>();
    }

    private void StartBlocking(InputAction.CallbackContext callbackContext)
    {
        if (_stateManager.State is FighterState.Neutral or FighterState.Moving)
        {
            OnStartBlocking?.Invoke();

            for (var i = 0; i < _limbRigControllers.Length; i++)
            {
                _limbRigControllers[i].SetTargetToFollow(_blockTargets[i]);
                StartCoroutine(StartBlockingCoroutine());
            }
        }
    }

    private IEnumerator StartBlockingCoroutine()
    {
        var timer = 0f;

        while (timer < _blockStartTime)
        {
            timer += Time.deltaTime;
            foreach (var limbRigController in _limbRigControllers)
            {
                limbRigController.SetRigWeight(timer / _blockStartTime);
            }
            yield return null;
        }
        
        _isBlocking = true;
    }

    private void StopBlocking(InputAction.CallbackContext callbackContext)
    {
        OnStopBlocking?.Invoke();
        StartCoroutine(StopBlockingCoroutine());
    }
    
    private IEnumerator StopBlockingCoroutine()
    {
        var timer = 0f;

        while (timer < _blockStartTime)
        {
            timer += Time.deltaTime;
            foreach (var limbRigController in _limbRigControllers)
            {
                limbRigController.SetRigWeight(1 - timer / _blockStartTime);
            }
            yield return null;
        }

        foreach (var limbRigController in _limbRigControllers)
        {
            limbRigController.TargetToFollow = _defaultTarget;
        }
        
        _isBlocking = false;
    }
}