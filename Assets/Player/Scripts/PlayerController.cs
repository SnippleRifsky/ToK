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

        private Transform _tracker;
        private Vector3 _trackerForward;
        private Vector3 _trackerRight;
        private Vector3 _trackerDirection;

        private void Awake()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            _characterController = GetComponent<CharacterController>();
            _camera = GetComponentInChildren<Camera>();
            _tracker = GameObject.FindGameObjectWithTag("CameraTracker").transform;
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
            _tracker.rotation = Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0);
            //TODO un-fuck this abomination
            var moveInput = _moveAction.ReadValue<Vector2>();

            if (_characterController.isGrounded)
            {
                _trackerForward = _tracker.transform.forward;
                _trackerRight = _tracker.transform.right;

                var trackerForwardVector = _trackerForward * moveInput.y;
                var trackerRightVector = _trackerRight * moveInput.x;

                _trackerDirection = trackerForwardVector + trackerRightVector;
                _trackerDirection.Normalize();

                _currentMovement.x = _trackerDirection.x;
                _currentMovement.z = _trackerDirection.z;
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