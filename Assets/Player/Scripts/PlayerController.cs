using UnityEngine;

namespace Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterController _characterController;
        private InputHandler _inputHandler;
        private Camera _camera;

        [Header("Movement Parameters")] [SerializeField]
        private float walkSpeed = 3f;

        [Header("Jump Parameters")] [SerializeField]
        private float jumpForce = 3f;

        [SerializeField] private float gravity = -9.81f;
        private Vector3 _currentMovement;

        [Header("Rotation Parameters")] [SerializeField]
        public float smoothTime = 0.2f;

        private Vector3 _cameraForward;
        private Vector3 _cameraRight;
        private Vector3 _camDirection;

        private void Awake()
        {
            _inputHandler = InputHandler.Instance;
            _characterController = GetComponent<CharacterController>();
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
            //TODO un-fuck this abomination

            if (_characterController.isGrounded)
            {
                _cameraForward = _camera.transform.forward;
                _cameraRight = _camera.transform.right;

                var verticalInput = _inputHandler.MoveInput.y;
                var horizontalInput = _inputHandler.MoveInput.x;

                var camForwardVector = _cameraForward * verticalInput;
                var camRightVector = _cameraRight * horizontalInput;

                _camDirection = camForwardVector + camRightVector;
                _camDirection.Normalize();

                _currentMovement.x = _camDirection.x;
                _currentMovement.z = _camDirection.z;
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
            var targetRotation = new Vector3(_currentMovement.x, 0, _currentMovement.z);
            transform.forward = Vector3.Slerp(transform.forward, targetRotation, Time.deltaTime / smoothTime);
        }
    }
}