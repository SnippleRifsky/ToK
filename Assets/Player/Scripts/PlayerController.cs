using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterController _characterController;
        private Transform _playerBody;
        private Camera _camera;

        [Header("Movement Parameters")] [SerializeField]
        private float walkSpeed = 3f;

        private InputAction _moveAction;
        private InputAction _jumpAction;

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
            _moveAction = InputSystem.actions.FindAction("Move");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            _characterController = GetComponent<CharacterController>();
            _camera = GetComponentInChildren<Camera>();
            _playerBody = GetComponentInChildren<MeshRenderer>().transform;
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
            var moveInput = _moveAction.ReadValue<Vector2>();

            if (_characterController.isGrounded)
            {
                _cameraForward = _camera.transform.forward;
                _cameraRight = _camera.transform.right;

                var camForwardVector = _cameraForward * moveInput.y;
                var camRightVector = _cameraRight * moveInput.x;

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
                if (_jumpAction.IsPressed()) _currentMovement.y = jumpForce;
            }
            else
            {
                _currentMovement.y += gravity * Time.deltaTime;
            }
        }

        private void HandleRotation()
        {
            if (_moveAction.ReadValue<Vector2>() == Vector2.zero) return;
            var targetRotation = new Vector3(_currentMovement.x, 0, _currentMovement.z);
            _playerBody.forward = Vector3.Slerp(_playerBody.forward, targetRotation, Time.deltaTime / smoothTime);
        }
    }
}