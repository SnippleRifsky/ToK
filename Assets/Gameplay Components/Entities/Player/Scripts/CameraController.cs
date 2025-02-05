using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    // Constants
    private const float CamClipOffset = 0.55f;
    private const float DistanceOffset = -0.2f;
    private const float RaycastSphereRadius = 0.5f;
    private const float ZoomMultiplier = -1f;

    [Header("Camera Movement Parameters")]
    [SerializeField] private float lookSensitivity = 0.5f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    [Header("Camera Zoom Parameters")]
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 20f;

    [Header("Camera Collision Parameters")]
    [SerializeField] private LayerMask _collisionLayerMask;

    [Header("Debug Variables")]
    [SerializeField] private bool isDebug;

    // Camera and Input
    private Camera _camera;
    private Vector2 _lookInput;
    private float _currentX = 0f;
    private float _currentY = 0f;
    private float _currentZoom = 10f;

    // Input Actions
    private InputAction _lookAction;
    private InputAction _panAction;
    private InputAction _zoomAction;
    private InputAction _camLockAction;

    // States
    private bool IsPanning { get; set; }
    public bool IsLocked { get; private set; }

    private RaycastHit _hit;

    private void Awake()
    {
        // Initialize input actions
        _lookAction = InputSystem.actions.FindAction("Look");
        _zoomAction = InputSystem.actions.FindAction("Zoom");
        _panAction = InputSystem.actions.FindAction("Pan");
        _camLockAction = InputSystem.actions.FindAction("Cam Lock");

        // Initialize camera
        if (!GameManager.Instance.IsInitialized)
        {
            Debug.LogWarning("CameraController: GameManager not initialized!");
        }

        _collisionLayerMask = 1 << LayerMask.NameToLayer("Default");
        _camera = GameManager.Instance.PlayerCamera;

        if (_camera == null)
        {
            Debug.LogError("CameraController: Camera reference missing!");
        }

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        HandleInput();
        HandleCameraRotation();
        HandleZoom();
        DetectOcclusion();
    }

    private void HandleCameraRotation()
    {
        if (IsPanning || IsLocked)
        {
            _currentX += _lookInput.x * lookSensitivity;
            _currentY += _lookInput.y * lookSensitivity;
            ClampCameraRotation();

            Vector3 direction = new Vector3(0, 0, -_currentZoom);
            Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
            _camera.transform.position = transform.position + rotation * direction;
        }

        _camera.transform.LookAt(transform.position);
    }

    private void HandleZoom()
    {
        float zoomInput = _zoomAction.ReadValue<Vector2>().y * ZoomMultiplier;
        _currentZoom = Mathf.Clamp(_currentZoom + zoomInput, minZoom, maxZoom);
    }

    private void DetectOcclusion()
    {
        Ray camRay = GetCameraRay();

        if (Physics.SphereCast(camRay, RaycastSphereRadius, out _hit, _currentZoom + DistanceOffset, _collisionLayerMask))
        {
            _camera.transform.position = _hit.point + _hit.normal.normalized * CamClipOffset;
        }
    }

    private Ray GetCameraRay()
    {
        Vector3 direction = (_camera.transform.position - transform.position).normalized;
        return new Ray(transform.position, direction);
    }

    private void HandleInput()
    {
        IsPanning = ShouldAllowPanning() && _panAction.ReadValue<float>() > 0;
        IsLocked = _camLockAction.ReadValue<float>() > 0;
        _lookInput = _lookAction.ReadValue<Vector2>();

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            SetCursorLock();
        }
    }

    private bool ShouldAllowPanning()
    {
        return !CursorRaycastService.Instance.IsCursorPointingAtEntity() && !EventSystem.current.IsPointerOverGameObject();
    }

    private void SetCursorLock()
    {
        Cursor.lockState = IsPanning || IsLocked ? CursorLockMode.Locked : CursorLockMode.Confined;
    }

    private void ClampCameraRotation()
    {
        _currentY = Mathf.Clamp(_currentY, minPitch, maxPitch);
    }
}