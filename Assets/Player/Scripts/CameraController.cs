using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [Header("Camera Input")]
    private InputAction _lookAction;
    private InputAction _zoomAction;
    
    [Header("Camera Movement Parameters")]
    [SerializeField] private float lookSensitivity = 0.5f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;
    private Camera _camera;
    private float _currentX;
    private float _currentY;
    
    [Header("Camera Zoom Parameters")]
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 20f;
    private float _distance;
    private float _zoomValue = 10f;

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
    } 

    private void CameraRotation()
    {
        Vector2 lookInput = _lookAction.ReadValue<Vector2>();
        
        _currentX += lookInput.x * lookSensitivity;
        _currentY += lookInput.y * lookSensitivity;

        _currentY = Mathf.Clamp(_currentY, minPitch, maxPitch);

        Vector3 direction = new Vector3(0, 0, -_distance);
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
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
}
