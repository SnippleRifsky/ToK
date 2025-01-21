using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterController _characterController;
        private CameraController _cameraController;
        private Transform _playerBody;
        private Camera _camera;

        [Header("Movement Parameters")] [SerializeField]
        private float walkSpeed = 3f;

        private readonly float _slopeSlideSpeed = 2f;
        private readonly float _groundRayDistance = 1f;
        private RaycastHit _slopeHit;

        private InputAction _moveAction;
        private InputAction _jumpAction;

        [Header("Jump Parameters")] [SerializeField]
        private float jumpForce = 3f;

        [SerializeField] private float gravity = -9.81f;
        private Vector3 _currentMovement;

        [Header("Rotation Parameters")] [SerializeField]
        public float smoothTime = 0.2f;

        private Transform _tracker;

        private void Awake()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            _characterController = GetComponent<CharacterController>();
            _camera = GetComponentInChildren<Camera>();
            _cameraController = GetComponentInChildren<CameraController>();
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
            var camLocked = _cameraController.IsLocked;
            if (_characterController.isGrounded && camLocked)
            {
                var trackerDirection = GetDirection(_tracker.transform);
                _currentMovement.x = trackerDirection.x;
                _currentMovement.z = trackerDirection.z;
            }
            else if (_characterController.isGrounded)
            {
                var playerDirection = GetDirection(_playerBody.transform);
                _currentMovement.x = playerDirection.x;
                _currentMovement.z = playerDirection.z;
            }

            if (OnSteepSlope()) SteepSlopeMovement();

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

        private Vector3 GetDirection(Transform target)
        {
            var targetDirection = (target.forward * _moveAction.ReadValue<Vector2>().y) +
                                  (target.right * _moveAction.ReadValue<Vector2>().x);
            targetDirection.Normalize();
            return targetDirection;
        }

        #region SlopeMovement

        private bool OnSteepSlope()
        {
            if (!_characterController.isGrounded) return false;
            var slopeRay = new Ray(_playerBody.position, Vector3.down);

            if (!Physics.SphereCast(slopeRay, _characterController.radius, out _slopeHit,
                    _characterController.height / 2 * _groundRayDistance)) return false;

            var slopeAngle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return slopeAngle >= _characterController.slopeLimit;
        }

        private void SteepSlopeMovement()
        {
            var slopeDirection = Vector3.up - _slopeHit.normal * Vector3.Dot(Vector3.up, _slopeHit.normal);
            var slideSpeed = walkSpeed - _slopeSlideSpeed;

            _currentMovement = slopeDirection * -slideSpeed;
            _currentMovement.y = _currentMovement.y - _slopeHit.point.y;
        }

        #endregion
    }
}