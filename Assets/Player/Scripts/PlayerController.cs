using UnityEngine;

namespace Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float jumpForce = 3f;
        [SerializeField] private float gravity = -9.81f;
        private Vector3 _currentMovement;
        [SerializeField] public float smoothTime = 0.2f;

        private CharacterController _characterController;
        private InputHandler _inputHandler;
        private Camera _camera;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _inputHandler = InputHandler.Instance;
            _camera = Camera.main;
        }

        private void Update()
        {
            HandleAllMovement();
        }

        private void HandleAllMovement()
        {
            HandleMovement();
            HandleJump();
            HandleRotation();
        }

        private void HandleMovement()
        {
            var inputDirection = new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y);

            if (_characterController.isGrounded)
            {
                _currentMovement.x = inputDirection.x;
                _currentMovement.z = inputDirection.z;
            }

            _characterController.Move(_currentMovement * (walkSpeed * Time.deltaTime));
        }

        private void HandleJump()
        {
            if (_characterController.isGrounded)
            {
                _currentMovement.y = -0.5f;
                if (_inputHandler.JumpTriggered) _currentMovement.y = jumpForce;
            }
            else
            {
                _currentMovement.y += gravity * Time.deltaTime;
            }
        }

        private void HandleRotation()
        {
            if (_inputHandler.MoveInput == Vector2.zero) return;
            var targetRotation = new Vector3(_inputHandler.MoveInput.x, 0, _inputHandler.MoveInput.y);
            transform.forward = Vector3.Slerp(transform.forward, targetRotation, Time.deltaTime / smoothTime);
        }
    }
}