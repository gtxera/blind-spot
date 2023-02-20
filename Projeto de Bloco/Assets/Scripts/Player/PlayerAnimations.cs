using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Walking = Animator.StringToHash("Walking");
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Driving = Animator.StringToHash("Driving");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerInputs.Instance.MovementStartedEvent += StartWalkingAnimation;
        PlayerInputs.Instance.MovementStoppedEvent += StopWalkAnimation;

        PlayerInputs.Instance.RunKeyDownEvent += StartRunningAnimation;
        PlayerInputs.Instance.RunKeyUpEvent += StopRunningAnimation;

        PlayerMovementStateMachine.Instance.StateChanged += StartDrivingAnimation;
        PlayerMovementStateMachine.Instance.StateChanged += StopDrivingAnimation;
        PlayerMovementStateMachine.Instance.StateChanged += StartIdleAnimation;
        PlayerMovementStateMachine.Instance.StateChanged += StopIdleAnimation;
    }

    private void StartWalkingAnimation()
    {
        if(PlayerMovementStateMachine.Instance.GetCurrentMovementStateType() == typeof(FootMovementState)) 
            _animator.SetBool(Walking, true);
    }

    private void StopWalkAnimation()
    {
        if(PlayerMovementStateMachine.Instance.GetCurrentMovementStateType() == typeof(FootMovementState)) 
            _animator.SetBool(Walking, false);
    }

    private void StartRunningAnimation()
    {
        if(PlayerMovementStateMachine.Instance.GetCurrentMovementStateType() == typeof(FootMovementState)) 
            _animator.SetBool(Running, true);
    }
    
    private void StopRunningAnimation()
    {
        if(PlayerMovementStateMachine.Instance.GetCurrentMovementStateType() == typeof(FootMovementState)) 
            _animator.SetBool(Running, false);
    }

    private void StartDrivingAnimation(Type oldState, Type newState)
    {
        if(newState == typeof(CarMovementState))
            _animator.SetBool(Driving, true);
    }
    
    private void StopDrivingAnimation(Type oldState, Type newState)
    {
        if(oldState == typeof(CarMovementState))
            _animator.SetBool(Driving, false);
    }

    private void StartIdleAnimation(Type oldState, Type newState)
    {
        if(newState == typeof(StaticMovementState)) _animator.SetBool(Walking, false);
    }

    private void StopIdleAnimation(Type oldState, Type newState)
    {
        if (newState == typeof(FootMovementState) && !PlayerInputs.Instance.MovementDirection.Equals(Vector3.zero)) _animator.SetBool(Walking, true);
    }
    
}
