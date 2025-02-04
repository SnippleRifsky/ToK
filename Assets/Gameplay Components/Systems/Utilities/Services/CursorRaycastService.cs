using System.Collections.Generic;
using UnityEngine;

public class CursorRaycastService : MonoBehaviour
{
    private const float RaycastMaxDistance = Mathf.Infinity;
    private static CursorRaycastService _instance;
    private Camera _mainCamera;
    private LayerMask _entityLayer;
    private Dictionary<Collider, Entity> _entityColliders = new();

    public static CursorRaycastService Instance
    {
        get
        {
            if (_instance is null) CreateInstance();
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        InitializeService();
    }

    private static void CreateInstance()
    {
        var serviceObject = new GameObject(nameof(CursorRaycastService));
        _instance = serviceObject.AddComponent<CursorRaycastService>();
        DontDestroyOnLoad(serviceObject);
    }

    private void InitializeService()
    {
        _entityLayer = LayerMask.GetMask("Entity");
        _mainCamera = GameManager.Instance.PlayerCamera;
    }

    private Ray GetCursorRay()
    {
        return _mainCamera.ScreenPointToRay(Input.mousePosition);
    }

    public bool TryGetEntityUnderCursor(out Entity entity)
    {
        var cursorRay = GetCursorRay();
        entity = null;

        return Physics.Raycast(cursorRay, out var hit, RaycastMaxDistance, _entityLayer) &&
               _entityColliders.TryGetValue(hit.collider, out entity);
    }

    public void RegisterEntity(Entity entity, Collider collider)
    {
        _entityColliders.TryAdd(collider, entity);
    }

    public void UnregisterEntity(Collider collider)
    {
        _entityColliders.Remove(collider);
    }

    public bool IsCursorPointingAtEntity()
    {
        var cursorRay = GetCursorRay();
        return Physics.Raycast(cursorRay, RaycastMaxDistance, _entityLayer);
    }
}