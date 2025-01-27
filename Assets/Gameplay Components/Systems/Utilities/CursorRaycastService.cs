using UnityEngine;

public class CursorRaycastService : MonoBehaviour
{
    private static CursorRaycastService _instance;
    private Camera _mainCamera;
    private LayerMask _entityLayer;

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
        GameObject go = new GameObject("CursorRaycastService");
        _instance = go.AddComponent<CursorRaycastService>();
        DontDestroyOnLoad(go);
    }

    public bool TryGetEntityUnderCursor(out Entity entity)
    {
        entity = null;
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _entityLayer)) return false;
        entity = hit.collider.GetComponent<Entity>();
        return entity != null;

    }

    public bool IsPointerOverEntity()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, Mathf.Infinity, _entityLayer);
    }
}