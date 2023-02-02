using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Player;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementStateMachine : SingletonBehaviour<PlayerMovementStateMachine>
{
    private MovementState _currentMovementState;

    private FootMovementState _defaultMovementState;
    
    [SerializeField] private float walkSpeed, runSpeed;
    private CharacterController _characterController;

    public event Action<Type, Type> StateChanged;
    
    private new void Awake()
    {
        base.Awake();
        
        _characterController = GetComponent<CharacterController>();
        
        _defaultMovementState = new FootMovementState(walkSpeed, runSpeed, _characterController, transform);
        _defaultMovementState.ActivateMovement(true);
        _currentMovementState = _defaultMovementState;
    }

    private void Update()
    {
        _currentMovementState.DoUpdate();
    }

    private void FixedUpdate()
    {
        _currentMovementState.DoFixedUpdate();
    }

    public void ChangeMovementState(MovementState newState)
    {
        StateChanged?.Invoke(_currentMovementState.GetType(), newState.GetType());
        _currentMovementState.ActivateMovement(false);
        
        _currentMovementState = newState;
        _currentMovementState.ActivateMovement(true);
        
    }

    public void ChangeDefaultMovementState()
    {
        StateChanged?.Invoke(_currentMovementState.GetType(), _defaultMovementState.GetType());
        _currentMovementState.ActivateMovement(false);
        
        _currentMovementState = _defaultMovementState;
        _currentMovementState.ActivateMovement(true);
        
    }

    public Type GetCurrentMovementStateType()
    {
        return _currentMovementState.GetType();
    }
}
