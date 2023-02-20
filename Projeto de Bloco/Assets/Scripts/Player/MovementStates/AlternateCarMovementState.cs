using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateCarMovementState : MovementState
{
    private Rigidbody _carRigidbody;

    private float _wheelSuspensionRestDistance, _wheelSuspensionForce, _wheelSpringDamper;

    private Transform[] _wheelTransforms = new Transform[4];

    public AlternateCarMovementState(Rigidbody carRigidbody, 
        Transform leftFrontWheelTransform, Transform rightFrontWheelTransform, Transform leftBackWheelTransform, Transform rightBackWheelTransform)
    {
        _carRigidbody = carRigidbody;

        _wheelTransforms[0] = leftFrontWheelTransform;
        _wheelTransforms[1] = rightFrontWheelTransform;
        _wheelTransforms[2] = leftBackWheelTransform;
        _wheelTransforms[3] = rightBackWheelTransform;
    }
    
    public override void DoFixedUpdate()
    {
        foreach (var wheelTransform in _wheelTransforms)
        {
            SuspensionForce(wheelTransform);
        }
    }

    private void SuspensionForce(Transform wheelTransform)
    {
        var rayDidHit = Physics.Raycast(wheelTransform.position, wheelTransform.TransformDirection(Vector3.down), out var tireRay, _wheelSuspensionRestDistance);

        if (rayDidHit)
        {
            Vector3 springDirection = wheelTransform.up;

            Vector3 tireWorldVelocity = _carRigidbody.GetPointVelocity(wheelTransform.position);

            float offset = _wheelSuspensionRestDistance - tireRay.distance;

            float velocity = Vector3.Dot(springDirection, tireWorldVelocity);

            float force = (offset * _wheelSuspensionForce) - (velocity * _wheelSpringDamper);
            
            _carRigidbody.AddForceAtPosition(springDirection * force, wheelTransform.position);
        }
    }
}
