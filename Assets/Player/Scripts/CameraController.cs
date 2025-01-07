using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Input")] private InputAction _lookAction;
    private InputAction _zoomAction;

    [Header("Camera Movement Parameters")] [SerializeField]
    private float lookSensitivity = 0.5f;

    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;
    private Camera _camera;
    private float _currentX;
    private float _currentY;

    [Header("Camera Zoom Parameters")] [SerializeField]
    private float minZoom = 2f;

    [SerializeField] private float maxZoom = 20f;
    private float _distance;
    private float _zoomValue = 10f;

    [Header("Camera Collision Parameters")] [SerializeField]
    private LayerMask layerMask;

    private const float CamClipOffset = 0.55f;
    private const float DistanceOffset = -0.2f;
    private RaycastHit _hit;

    [Header("Debug Variables")] [SerializeField]
    private bool isDebug;

    private void Awake()
    {
        _lookAction = InputSystem.actions.FindAction("Look");
        _zoomAction = InputSystem.actions.FindAction("Zoom");
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        CameraRotation();
        Zoom();
        DetectOcclusion();
    }

    private void CameraRotation()
    {
        var lookInput = _lookAction.ReadValue<Vector2>();

        _currentX += lookInput.x * lookSensitivity;
        _currentY += lookInput.y * lookSensitivity;

        _currentY = Mathf.Clamp(_currentY, minPitch, maxPitch);

        var direction = new Vector3(0, 0, -_distance);
        var rotation = Quaternion.Euler(_currentY, _currentX, 0);
        _camera.transform.position = gameObject.transform.position + rotation * direction;

        _camera.transform.LookAt(gameObject.transform.position);
    }

    private void Zoom()
    {
        var zoomInput = _zoomAction.ReadValue<Vector2>();
        _zoomValue += zoomInput.y * -1;
        _zoomValue = Mathf.Clamp(_zoomValue, minZoom, maxZoom);
        _distance = _zoomValue;
    }

    private void DetectOcclusion()
    {
        var playerPosition = gameObject.transform.position;
        var camRay = new Ray(playerPosition,
            (_camera.transform.position - gameObject.transform.position).normalized);

        if (!Physics.SphereCast(camRay, 0.5f, out _hit, _distance + DistanceOffset, layerMask)) return;
        _camera.transform.position = _hit.point + _hit.normal.normalized * CamClipOffset;
    }
}