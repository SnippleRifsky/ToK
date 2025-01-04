using System;
using UnityEngine;

namespace Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float jumpForce = 3f;
        [SerializeField] private float gravity = -9.81f;
        private Vector3 _currentMovement;
    
        private CharacterController _characterController;
        private InputHandler _inputHandler;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _inputHandler = InputHandler.Instance;
        }

        private void Update()
        {
            HandleAllMovement();
        }

        private void HandleAllMovement()
        {
            HandleMovement();
            HandleJump();
        }

        private void HandleMovement()
        {
            var inputDirection = new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y);
            var worldDirection = transform.TransformDirection(inputDirection).normalized;

            if (_characterController.isGrounded)
            {
                _currentMovement.x = worldDirection.x;
                _currentMovement.z = worldDirection.z;
            }

            _characterController.Move(_currentMovement * (walkSpeed * Time.deltaTime));
        }

        private void HandleJump()
        {
            if (_characterController.isGrounded)
            {
                _currentMovement.y = -0.5f;
                if (_inputHandler.JumpTriggered)
                {
                    _currentMovement.y = jumpForce;
                }
            }
            else
            {
                _currentMovement.y += gravity * Time.deltaTime;
            }
        }
    }
}
