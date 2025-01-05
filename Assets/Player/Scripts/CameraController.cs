using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private InputAction _lookAction;
    private Transform _pivot;
    private Camera _camera;
    private float _minPitch = -80f;
    private float _maxPitch = 80f;
    private float _distance = 10f;
    private float _currentX;
    private float _currentY;
    [SerializeField] private float lookSensitivity = 0.5f;

    private void Awake()
    {
        _lookAction = InputSystem.actions.FindAction("Look");
        _pivot = transform;
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        CameraRotation();
    } 

    private void CameraRotation()
    {
        Vector2 lookInput = _lookAction.ReadValue<Vector2>();
        
        _currentX += lookInput.x * lookSensitivity;
        _currentY += lookInput.y * lookSensitivity;

        _currentY = Mathf.Clamp(_currentY, _minPitch, _maxPitch);

        Vector3 direction = new Vector3(0, 0, -_distance);
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
        _camera.transform.position = gameObject.transform.position + rotation * direction;

        _camera.transform.LookAt(gameObject.transform.position);
    }
}
