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

        PlayerInputs.Instance.ShiftKeyDownEvent += StartRunningAnimation;
        PlayerInputs.Instance.ShiftKeyUpEvent += StopRunningAnimation;

        PlayerMovementStateMachine.Instance.StateChanged += StartDrivingAnimation;
        PlayerMovementStateMachine.Instance.StateChanged += StopDrivingAnimation;
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
    
}
