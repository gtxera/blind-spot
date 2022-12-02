using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Car")]
public class CarStats : ScriptableObject
{
    public float MaxMotorForce, MaxSpeedKmPerHour, MaxBreakForce, MaxSteerAngleInDegrees, AntiRollForce, DecelerationForce;
}
