using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class TargetController : MonoBehaviour
{
    [SerializeField] private TargetController _opponent;
    [SerializeField] private Rig _headRig;

    [Header("Self Targets")] 
    public Transform Head => _head;
    [SerializeField] private Transform _head;
    public Transform Body => _body;
    [SerializeField] private Transform _body;
    public Transform Legs => _legs;
    [SerializeField] private Transform _legs;
    
    [Header("IK Targets")]
    public Transform Aim => _aim;
    [SerializeField] private Transform _aim;

    [SerializeField] private float _lerpTargetDuration = 0.3f;
    [SerializeField] private AnimationCurve _curve;
    
    private StateManager _stateManager;

    // Inputs
    private PlayerInput _input;   
    private InputAction _lowAimAction;
    
    // Cached values
    private bool _isLerping; 
    private bool _aimLow;
    private bool _lerpExecuted = true;

    private void Start()
    {
        if (!_opponent)
        {
            Debug.LogError("opponent object not set");
            enabled = false;
        }

        _stateManager = GetComponent<StateManager>();
        _stateManager.OnStateChange += CheckDeath;

        _input = GetComponent<PlayerInput>();
        _lowAimAction = _input.actions.FindAction("AimLow");

        _lowAimAction.performed += StartAim;
        _lowAimAction.canceled += StopAim;
    }

    private void StartAim(InputAction.CallbackContext callbackContext)
    {
        _aimLow = true;
        if (_stateManager.State is FighterState.Startup or FighterState.Active or FighterState.Recover)
        {
            _lerpExecuted = false;
            return;
        }
        StopAllCoroutines();
        StartCoroutine(StartAimCoroutine());
    }

    private IEnumerator StartAimCoroutine()
    {
        var timer = 0f;
        var startPos = _aim.position;
        _isLerping = true;

        while (timer < _lerpTargetDuration)
        {
            timer += Time.deltaTime;
            _aim.position = Vector3.Lerp(startPos, _opponent.Body.position, _curve.Evaluate(timer / _lerpTargetDuration));
            yield return null;
        }
        
        _isLerping = false;
        _lerpExecuted = true;
        _lerpExecuted = true;
    }
    
    private void StopAim(InputAction.CallbackContext callbackContext)
    {
        _aimLow = false;
        if (_stateManager.State is FighterState.Startup or FighterState.Active or FighterState.Recover)
        {
            _lerpExecuted = false;
            return;
        }
        
        StopAllCoroutines();
        StartCoroutine(StopAimCoroutine());
    }
    
    private IEnumerator StopAimCoroutine()
    {
        var timer = 0f;
        var startPos = _aim.position;
        _isLerping = true;

        while (timer < _lerpTargetDuration)
        {
            timer += Time.deltaTime;
            _aim.position = Vector3.Lerp(startPos, _opponent.Head.position, _curve.Evaluate(timer / _lerpTargetDuration));
            yield return null;
        }
        
        _isLerping = false;
        _lerpExecuted = true;
    }

    private void Update()
    {
        if (_isLerping || _stateManager.State is FighterState.Startup or FighterState.Active or FighterState.Recover)
            return;

        if (!_lerpExecuted)
        {
            if(_aimLow)
                StartAim(new InputAction.CallbackContext());
            else
                StopAim(new InputAction.CallbackContext());
            return;
        }

        _aim.position = _aimLow ? _opponent.Body.position : _opponent.Head.position;
    }

    public void SetOpponent(TargetController opponent)
    {
        if(!_opponent)
            _opponent =  opponent;  
    }

    private void CheckDeath(FighterState state)
    {
        if (state != FighterState.Celebrating)
            return;
        
        _headRig.weight = 0;
    }

    private void OnDestroy()
    {
        _stateManager.OnStateChange -= CheckDeath;

        _lowAimAction.performed -= StartAim;
        _lowAimAction.canceled -= StopAim;
    }
}
