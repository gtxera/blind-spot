using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCar : MonoBehaviour
{
    [SerializeField] private WheelCollider _rearLeftWC;
    [SerializeField] private WheelCollider _frontLeftWC;
    [SerializeField] private WheelCollider _rearRightWC;
    [SerializeField] private WheelCollider _frontRightWC;

    [SerializeField] private Transform _rearLeftTrans;
    [SerializeField] private Transform _frontLeftTrans;
    [SerializeField] private Transform _rearRightTrans;
    [SerializeField] private Transform _frontRightTrans;

    [SerializeField] private float _motorTorque;

    private WheelCollider[] _wheels;
    private Transform[] _transforms;
    
    void Start()
    {
        _wheels = new[] { _rearLeftWC, _frontLeftWC, _rearRightWC, _frontRightWC };
        _transforms = new[] {_rearLeftTrans,  _frontLeftTrans, _rearRightTrans, _frontRightTrans};

        _rearLeftWC.motorTorque = _motorTorque;
        _rearRightWC.motorTorque = _motorTorque;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            _wheels[i].GetWorldPose(out var pos, out var rot);
            _transforms[i].rotation = rot;
            _transforms[i].position = pos;
        }
    }
}
