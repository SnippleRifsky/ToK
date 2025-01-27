using System.Collections.Generic;
using UnityEngine;

public class CursorRaycastService : MonoBehaviour
{
    private static CursorRaycastService _instance;
    private Camera _mainCamera;
    private LayerMask _entityLayer;
    private Dictionary<Collider, Entity> _entityCache = new Dictionary<Collider, Entity>();

    public static CursorRaycastService Instance
    {
        get
        {
            if (_instance is null)
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                // Should never be called unless the instance is null
                CreateInstance();
            }
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
        _entityLayer = LayerMask.GetMask("Entity");
        _mainCamera = Camera.main;
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private static void CreateInstance()
    {
        var go = new GameObject("CursorRaycastService");
        _instance = go.AddComponent<CursorRaycastService>();
        DontDestroyOnLoad(go);
    }

    public bool TryGetEntityUnderCursor(out Entity entity)
    {
        entity = null;
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        
        return Physics.Raycast(ray, out var hit, Mathf.Infinity, _entityLayer) &&
               _entityCache.TryGetValue(hit.collider, out entity);
    }
    
    // Called by Entity when it's created/enabled
    public void RegisterEntity(Entity entity, Collider entityCollider)
    {
        _entityCache.TryAdd(entityCollider, entity);
    }

    // Called by Entity when it's destroyed/disabled
    public void UnregisterEntity(Collider entityCollider)
    {
        if (!_entityCache.ContainsKey(entityCollider)) return;
        _entityCache.Remove(entityCollider);
    }

    public bool IsPointerOverEntity()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, Mathf.Infinity, _entityLayer);
    }
}