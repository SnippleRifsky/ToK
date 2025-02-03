using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Parameters")] 
    [SerializeField] private float walkSpeed = 3f;

    [Header("Jump Parameters")] 
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Rotation Parameters")] 
    [SerializeField] private float smoothTime = 0.2f;

    private const float GroundedYVelocity = -0.5f;
    private const float SlopeRayMultiplier = 1.1f;

    private readonly float _groundCheckDistance = 1f;
    private readonly float _slopeSlideSpeed = 2f;

    // References
    private Camera _camera;
    private CameraController _cameraController;
    private CharacterController _characterController;
    private Player _player;
    private Transform _playerBody;
    private Transform _tracker;

    // Input Actions
    private InputAction _jumpAction;
    private InputAction _moveAction;

    // State
    private Vector3 _currentMovement;
    private RaycastHit _slopeHit;

    private void Start()
    {
        _player = GameManager.Instance.Player;
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
        _characterController = GetComponent<CharacterController>();
        _camera = GameManager.Instance.PlayerCamera;
        _cameraController = _player.CameraController;
        _tracker = GameObject.FindGameObjectWithTag("CameraTracker").transform;
        _playerBody = GetComponentInChildren<MeshRenderer>().transform;
    }

    private void Update()
    {
        ProcessMovement();
    }

    private void ProcessMovement()
    {
        ProcessHorizontalMovement();
        ProcessVerticalMovement();
        ProcessPlayerRotation();
    }

    private void ProcessHorizontalMovement()
    {
        AlignTrackerToCamera();

        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        bool isCameraLocked = _cameraController.IsLocked;

        // Calculate movement direction
        if (_characterController.isGrounded)
        {
            Transform reference = isCameraLocked ? _tracker : _playerBody;
            Vector3 direction = GetMovementDirection(reference);
            _currentMovement.x = direction.x;
            _currentMovement.z = direction.z;
        }

        if (IsOnSteepSlope())
        {
            HandleSteepSlopeMovement();
        }

        // Apply movement
        _characterController.Move(_currentMovement * (walkSpeed * Time.deltaTime));
    }

    private void ProcessVerticalMovement()
    {
        if (_characterController.isGrounded)
        {
            _currentMovement.y = GroundedYVelocity;

            if (_jumpAction.IsPressed())
            {
                _currentMovement.y = jumpForce;
            }
        }
        else
        {
            _currentMovement.y += gravity * Time.deltaTime;
        }
    }

    private void ProcessPlayerRotation()
    {
        if (_moveAction.ReadValue<Vector2>() == Vector2.zero) return;

        Vector3 targetRotation = CalculateRotationTarget();

        _playerBody.forward = Vector3.Slerp(
            _playerBody.forward,
            targetRotation,
            Time.deltaTime / smoothTime
        );
    }

    private void AlignTrackerToCamera()
    {
        _tracker.rotation = Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0);
    }

    private Vector3 GetMovementDirection(Transform reference)
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 direction = reference.forward * moveInput.y + reference.right * moveInput.x;
        direction.Normalize();
        return direction;
    }

    private Vector3 CalculateRotationTarget()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 targetRotation = new Vector3(_currentMovement.x, 0, _currentMovement.z);

        if (!_cameraController.IsLocked && moveInput.y < 0)
        {
            targetRotation = -targetRotation;
        }

        return targetRotation;
    }

    #region Slope Movement
    private bool IsOnSteepSlope()
    {
        if (!_characterController.isGrounded) return false;

        Ray slopeRay = new Ray(_playerBody.position, Vector3.down);
        return Physics.SphereCast(
            slopeRay,
            _characterController.radius * SlopeRayMultiplier,
            out _slopeHit,
            _characterController.height / 2 * _groundCheckDistance
        ) && Vector3.Angle(Vector3.up, _slopeHit.normal) >= _characterController.slopeLimit;
    }

    private void HandleSteepSlopeMovement()
    {
        Vector3 slopeDirection = GetSlopeInfo();
        float slideSpeed = walkSpeed - _slopeSlideSpeed;

        _currentMovement = slopeDirection * -slideSpeed;
        _currentMovement.y -= _slopeHit.point.y;
    }

    private Vector3 GetSlopeInfo()
    {
        return Vector3.up - _slopeHit.normal * Vector3.Dot(Vector3.up, _slopeHit.normal);
    }
    #endregion
}