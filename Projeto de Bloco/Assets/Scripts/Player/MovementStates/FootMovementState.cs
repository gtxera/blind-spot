using System;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class FootMovementState : MovementState
    {
        private float _walkSpeed, _runSpeed;
        private float _currentSpeed;

        private Quaternion _currentRotation, _lastRotation;
        private Vector3 _moveDirection;
        
        private bool _isRunning;
        
        private CharacterController _characterController;

        private Transform _playerTransform;

        private const float ANGLE_STEP = 45f;
        private readonly Vector3 ISOMETRIC_LEFT = new (-1, 0, 1);


        public FootMovementState(float walkSpeed, float runSpeed, CharacterController characterController, Transform playerTransform)
        {
            _walkSpeed = walkSpeed;
            _runSpeed = runSpeed;
            _characterController = characterController;
            _playerTransform = playerTransform;
            _lastRotation = playerTransform.rotation;

            PlayerInputs.Instance.ShiftKeyDownEvent += () =>
            {
                if (_isActive) _isRunning = true;
            };
            PlayerInputs.Instance.ShiftKeyUpEvent += () =>
            {
                if (_isActive) _isRunning = false;
            };
        }
        
        public override void DoFixedUpdate()
        {   
            if (!_isActive) return;
            
            HandleRotation();
            HandleMovement();
        }

        private void HandleRotation()
        {
            if (PlayerInputs.Instance.MovementDirection.normalized.magnitude > 0)
            {
                float angle = Vector3.SignedAngle(ISOMETRIC_LEFT, PlayerInputs.Instance.MovementDirection, Vector3.up);

                float timesToRotate = angle / ANGLE_STEP;

                _lastRotation = Quaternion.Euler(0, ANGLE_STEP * timesToRotate, 0);

                _moveDirection = _lastRotation * Vector3.forward;
            }

            _currentRotation = Quaternion.Slerp(_currentRotation, _lastRotation, 0.2f);
            _playerTransform.rotation = _currentRotation;
        }
        
        private void HandleMovement()
        {
            float currentMaxSpeed = _isRunning ? _runSpeed : _walkSpeed;
            currentMaxSpeed = PlayerInputs.Instance.MovementDirection.magnitude == 0 ? 0f : currentMaxSpeed;
            _currentSpeed = Mathf.Lerp(_currentSpeed, currentMaxSpeed * PlayerInputs.Instance.MovementDirection.normalized.magnitude, 0.1f);

            _characterController.SimpleMove(_currentSpeed * _moveDirection);
        }

        public override void ResetMovement()
        {
            _currentSpeed = 0;
        }
    }
}
