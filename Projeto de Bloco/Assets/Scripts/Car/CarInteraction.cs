using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private CarStats carStats;

    private Rigidbody _carRigidBody;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private Transform driverSeatTransform, exitPointTransform;
    
    private CarMovementState _carMovementState;

    private void Awake()
    {
        _carRigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _carMovementState = new CarMovementState(
            _carRigidBody,
            carStats.MaxMotorForce, carStats.MaxSpeedKmPerHour, carStats.MaxBreakForce, carStats.MaxSteerAngleInDegrees, carStats.AntiRollForce, carStats.DecelerationForce,
            frontLeftWheelCollider, frontRightWheelCollider, rearLeftWheelCollider, rearRightWheelCollider,
            frontLeftWheelTransform, frontRightWheelTransform, rearLeftWheelTransform, rearRightWheelTransform);
    }


    public void Interact(GameObject playerObject)
    {
        if (PlayerMovementStateMachine.Instance.GetCurrentMovementStateType() != typeof(CarMovementState))
        {
            _carMovementState.SetDriverAndSeatTransform(playerObject.transform, driverSeatTransform);
            playerObject.GetComponent<CharacterController>().enabled = false;
            PlayerMovementStateMachine.Instance.ChangeMovementState(_carMovementState);
            PlayerInteraction.Instance.LockInteraction(true);
        }
        else
        {
            playerObject.transform.position = exitPointTransform.position;
            playerObject.GetComponent<CharacterController>().enabled = true;
            _carMovementState.FullStop();
            PlayerMovementStateMachine.Instance.ChangeDefaultMovementState();
            PlayerInteraction.Instance.LockInteraction(false);
        }
        
    }
}
