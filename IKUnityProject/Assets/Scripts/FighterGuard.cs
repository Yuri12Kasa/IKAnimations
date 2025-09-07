using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FighterGuard : MonoBehaviour
{
    public Action OnStartBlocking;
    public Action OnStopBlocking;

    [SerializeField] private float _changePostureDuration = 0.1f;
    
    [SerializeField] private LimbRigController[] _limbRigControllers = new LimbRigController[2];

    [SerializeField] private Transform[] _blockHighPoints = new Transform[2];
    [SerializeField] private Transform[] _blockLowPoints = new Transform[2];
    [SerializeField] private Transform[] _blockTargets = new Transform[2];
    
    [SerializeField] private HitReact _bodyHitReact;
    [SerializeField] private HitReact _headHitReact;

    private bool _blockingLow;
    
    private PlayerInput _playerInput;
    private InputAction _blockAction;
    private InputAction _lowAimAction;
    
    private Transform _defaultTarget;
    private StateManager _stateManager;

    private Coroutine _currentCoroutine;

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
        
        _lowAimAction = _playerInput.actions.FindAction("AimLow");

        _lowAimAction.performed += BlockLow;
        _lowAimAction.canceled += BlockHigh;
        
        _defaultTarget = GetComponent<TargetController>().Aim;
        _stateManager = GetComponentInParent<StateManager>();
    }

    private void Update()
    {
        if (_stateManager.State == FighterState.Blocking)
            return;
        
        if (_lowAimAction.IsPressed())
        {
            _blockingLow = true;
            _blockTargets[0].position = _blockLowPoints[0].position;
            _blockTargets[1].position = _blockLowPoints[1].position;
        }
        else
        {
            _blockingLow = false;
            _blockTargets[0].position = _blockHighPoints[0].position;
            _blockTargets[1].position = _blockHighPoints[1].position;
        }
    }

    private void StartBlocking(InputAction.CallbackContext callbackContext)
    {
        if (_stateManager.State is not (FighterState.Neutral or FighterState.Moving)) 
            return;
        
        OnStartBlocking?.Invoke();

        for (var i = 0; i < _limbRigControllers.Length; i++)
        {
            _limbRigControllers[i].SetTargetToFollow(_blockTargets[i]);
            if(_currentCoroutine != null) 
                StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(StartBlockingCoroutine());
        }
    }

    private IEnumerator StartBlockingCoroutine()
    {
        var timer = 0f;

        while (timer < _changePostureDuration)
        {
            timer += Time.deltaTime;
            foreach (var limbRigController in _limbRigControllers)
            {
                limbRigController.SetRigWeight(timer / _changePostureDuration);
            }
            yield return null;
        }
        
        if (_blockingLow)
        {
            _bodyHitReact.DisableTrigger();
        }
        else
        {
            _headHitReact.DisableTrigger();
        }
        
        _currentCoroutine = null;
    }

    private void StopBlocking(InputAction.CallbackContext callbackContext)
    {
        if (_stateManager.State is not FighterState.Blocking)
            return;
        
        if(_currentCoroutine != null) 
            StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(StopBlockingCoroutine());
    }
    
    private IEnumerator StopBlockingCoroutine()
    {
        var timer = 0f;

        while (timer < _changePostureDuration)
        {
            timer += Time.deltaTime;
            foreach (var limbRigController in _limbRigControllers)
            {
                limbRigController.SetRigWeight(1 - timer / _changePostureDuration);
            }
            yield return null;
        }

        foreach (var limbRigController in _limbRigControllers)
        {
            limbRigController.TargetToFollow = _defaultTarget;
        }
        
        _blockTargets[0].position = _blockHighPoints[0].position;
        _blockTargets[1].position = _blockHighPoints[1].position;
        
        _bodyHitReact.EnableTrigger();
        _headHitReact.EnableTrigger();
        
        OnStopBlocking?.Invoke();
        
        _currentCoroutine = null;
    }

    private void BlockHigh(InputAction.CallbackContext callbackContext)
    {
        if(_stateManager.State is not FighterState.Blocking)
            return;

        if(_currentCoroutine != null) 
            StopCoroutine(_currentCoroutine);
        
        _bodyHitReact.EnableTrigger();
        _headHitReact.DisableTrigger();
        
        _blockingLow = false;
        _currentCoroutine = StartCoroutine(LerpGuardTargets(_blockHighPoints));
    }
    
    private void BlockLow(InputAction.CallbackContext callbackContext)
    {
        if(_stateManager.State is not FighterState.Blocking)
            return;

        if(_currentCoroutine != null) 
            StopCoroutine(_currentCoroutine);
        
        _bodyHitReact.DisableTrigger();
        _headHitReact.EnableTrigger();
        
        _blockingLow = true;
        _currentCoroutine = StartCoroutine(LerpGuardTargets(_blockLowPoints));
    }

    private IEnumerator LerpGuardTargets(Transform[] targets)
    {
        var timer = 0f;
        
        var startPos1 = _blockTargets[0].position;
        var startPos2 = _blockTargets[1].position;

        while (timer < _changePostureDuration)
        {
            timer += Time.deltaTime;
            _blockTargets[0].position = Vector3.Lerp(startPos1, targets[0].position, timer / _changePostureDuration);
            _blockTargets[1].position = Vector3.Lerp(startPos2, targets[1].position, timer / _changePostureDuration);
            
            yield return null;
        }
        
        _blockTargets[0].position = targets[0].position;
        _blockTargets[1].position = targets[1].position;
        
        _currentCoroutine =  null;   
    }
}