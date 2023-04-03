using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class CarMovementState : MovementState
{
    private float _maxMotorForce, _maxSpeedKmPerHour, _maxBreakForce, _maxSteerAngle, _antiRollForce, _decelerationForce;
    private float _currentMotorForce, _currentBreakForce, _currentSpeedKmPerHour, _currentSteerAngle, _currentDecelerationForce;
    private float _maxGasCapacity, _currentGasCapacity, _gasConsumption;

    private Rigidbody _carRigidBody;
    
    private WheelCollider _frontLeftWheelCollider;
    private WheelCollider _frontRightWheelCollider;
    private WheelCollider _rearLeftWheelCollider;
    private WheelCollider _rearRightWheelCollider;

    private Transform _frontLeftWheelTransform;
    private Transform _frontRightWheelTransform;
    private Transform _rearLeftWheelTransform;
    private Transform _rearRightWheelTransform;

    private Transform _driverTransform, _driverSeatTransform;

    public CarMovementState(
        Rigidbody carRigidBody,
        CarStats carStats,
        WheelCollider frontLeftWheelCollider, WheelCollider frontRightWheelCollider, WheelCollider rearLeftWheelCollider, WheelCollider rearRightWheelCollider,
        Transform frontLeftWheelTransform, Transform frontRightWheelTransform, Transform rearLeftWheelTransform, Transform rearRightWheelTransform)
    {
        _carRigidBody = carRigidBody;
        
        _maxMotorForce = carStats.MaxMotorForce;
        _maxSpeedKmPerHour = carStats.MaxSpeedKmPerHour;
        _maxBreakForce = carStats.MaxBreakForce;
        _maxSteerAngle = carStats.MaxSteerAngleInDegrees;
        _antiRollForce = carStats.AntiRollForce;
        _decelerationForce = carStats.DecelerationForce;

        _maxGasCapacity = carStats.MaximumGasCapacity;
        _currentGasCapacity = _maxGasCapacity;
        _gasConsumption = carStats.GasConsumptionInUnitsPerSecond;

        _frontLeftWheelCollider = frontLeftWheelCollider;
        _frontRightWheelCollider = frontRightWheelCollider;
        _rearLeftWheelCollider = rearLeftWheelCollider;
        _rearRightWheelCollider = rearRightWheelCollider;

        _frontLeftWheelTransform = frontLeftWheelTransform;
        _frontRightWheelTransform = frontRightWheelTransform;
        _rearLeftWheelTransform = rearLeftWheelTransform;
        _rearRightWheelTransform = rearRightWheelTransform;
    }
    
    public override void DoFixedUpdate()
    {
        if (!_isActive && _currentGasCapacity <= 0) return;
        
        HandleMotor();
        HandleDeceleration();
        HandleBreaking();
        HandleSteering();
        HandeAntiRollBars();
        HandleWheelsTransform();
        UpdateDriverPosition();
    }
    
    private void HandleMotor()
    {
        _currentMotorForce = Mathf.Lerp(_currentMotorForce, _maxMotorForce * PlayerInputs.Instance.VerticalInput, 0.01f);

        _currentSpeedKmPerHour = _carRigidBody.velocity.magnitude * 3.6f;

        if (Mathf.Abs(_currentSpeedKmPerHour) < _maxSpeedKmPerHour)
        {
            _frontLeftWheelCollider.motorTorque = _currentMotorForce;
            _frontRightWheelCollider.motorTorque = _currentMotorForce;
        }
        else
        {
            _frontLeftWheelCollider.motorTorque = 0f;
            _frontRightWheelCollider.motorTorque = 0f;
        }
    }

    private void ConsumeGas()
    {
        _currentGasCapacity -= _gasConsumption * Time.fixedDeltaTime;
    }

    private void HandleDeceleration()
    {
        _currentDecelerationForce = PlayerInputs.Instance.VerticalInput == 0 ? _decelerationForce : 0f;
    }

    private void HandleBreaking()
    {
        _currentBreakForce = PlayerInputs.Instance.CarBreaksPressed ? _maxBreakForce : 0f;

        _frontLeftWheelCollider.brakeTorque = _currentBreakForce + _currentDecelerationForce;
        _frontRightWheelCollider.brakeTorque = _currentBreakForce + _currentDecelerationForce;
        _rearLeftWheelCollider.brakeTorque = _currentBreakForce + _currentDecelerationForce;
        _rearRightWheelCollider.brakeTorque = _currentBreakForce + _currentDecelerationForce;
    }

    private void HandleSteering()
    {
        _currentSteerAngle = Mathf.Lerp(_currentSteerAngle, _maxSteerAngle * PlayerInputs.Instance.HorizontalInput, Time.fixedDeltaTime);

        _frontLeftWheelCollider.steerAngle = _currentSteerAngle;
        _frontRightWheelCollider.steerAngle = _currentSteerAngle;
    }

    private void HandeAntiRollBars()
    {
        AntiRollBar(_frontLeftWheelCollider, _frontRightWheelCollider);
        AntiRollBar(_rearLeftWheelCollider, _rearRightWheelCollider);
    }

    private void AntiRollBar(WheelCollider leftWheel, WheelCollider rightWheel)
    {
        WheelHit wheelHit;
        
        float leftTravel = 1.0f;
        bool leftIsGrounded = leftWheel.GetGroundHit(out wheelHit);
        if (leftIsGrounded)
        {
            leftTravel = (-leftWheel.transform.InverseTransformPoint(wheelHit.point).y - leftWheel.radius) /
                         leftWheel.suspensionDistance;
        }

        float rightTravel = 1.0f;
        bool rightIsGrounded = rightWheel.GetGroundHit(out wheelHit);
        if (rightIsGrounded)
        {
            rightTravel = (-rightWheel.transform.InverseTransformPoint(wheelHit.point).y - rightWheel.radius) /
                          rightWheel.suspensionDistance;
        }

        float currentAntiRollForce = (leftTravel - rightTravel) * _antiRollForce;

        if (leftIsGrounded)
        {
            _carRigidBody.AddForceAtPosition(leftWheel.transform.up * -currentAntiRollForce, leftWheel.transform.position);
        }

        if (rightIsGrounded)
        {
            _carRigidBody.AddForceAtPosition(rightWheel.transform.up * currentAntiRollForce, rightWheel.transform.position);        }
    }

    private void HandleWheelsTransform()
    {
        UpdateWheelTransform(_frontLeftWheelCollider, _frontLeftWheelTransform);
        UpdateWheelTransform(_frontRightWheelCollider, _frontRightWheelTransform);
        UpdateWheelTransform(_rearLeftWheelCollider, _rearLeftWheelTransform);
        UpdateWheelTransform(_rearRightWheelCollider, _rearRightWheelTransform);
    }

    private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 wheelPosition;
        Quaternion wheelRotation;
        
        wheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);

        wheelTransform.position = wheelPosition;
        wheelTransform.rotation = wheelRotation;
    }

    public void FullStop()
    {
        _frontLeftWheelCollider.motorTorque = 0f;
        _frontRightWheelCollider.motorTorque = 0f;
        
        _frontLeftWheelCollider.brakeTorque = _maxBreakForce;
        _frontRightWheelCollider.brakeTorque = _maxBreakForce;
        _rearLeftWheelCollider.brakeTorque = _maxBreakForce;
        _rearRightWheelCollider.brakeTorque = _maxBreakForce;
    }

    private void UpdateDriverPosition()
    {
        _driverTransform.position = _driverSeatTransform.position;
        _driverTransform.rotation = _driverSeatTransform.rotation;
    }

    public void SetDriverAndSeatTransform(Transform driverTransform, Transform seatTransform)
    {
        _driverTransform = driverTransform;
        _driverSeatTransform = seatTransform;
    }
    
}
