using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance is not null) return _instance;
            _instance = FindFirstObjectByType<UIManager>();
            if (_instance is not null) return _instance;
            var go = new GameObject("UIManager");
            _instance = go.AddComponent<UIManager>();

            return _instance;
        }
    }

    public CharacterPanel CharacterPanel { get; private set; }
    public TargetPanel TargetPanel { get; private set; }
    public Canvas UICanvas { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeUIReferences();
    }

    private void InitializeUIReferences()
    {
        CharacterPanel = FindFirstObjectByType<CharacterPanel>();
        if (CharacterPanel is null)
        {
            Debug.LogError("UIManager: CharacterPanel not found!");
            return;
        }

        TargetPanel = FindFirstObjectByType<TargetPanel>();
        if (TargetPanel is null) Debug.LogError("UIManager: TargetPanel not found!");

        UICanvas = FindFirstObjectByType<Canvas>();
    }

    #region EntityNameplate Logic

    public GameObject UIEntityNameplatePrefab;
    private readonly Queue<UIEntityNameplate> _nameplatePool = new();
    private readonly Dictionary<Entity, UIEntityNameplate> _activeNameplates = new();

    public void ShowEntityNameplate(Entity entity)
    {
        if (_activeNameplates.ContainsKey(entity)) return;

        var nameplate = GetNameplateFromPool();
        nameplate.Setup(entity);
        UpdateNameplatePosition(nameplate, entity);
        _activeNameplates[entity] = nameplate;
    }

    public void HideEntityNameplate(Entity entity)
    {
        if (_activeNameplates.TryGetValue(entity, out var nameplate))
        {
            ReturnNameplateToPool(nameplate);
            _activeNameplates.Remove(entity);
        }
    }

    private UIEntityNameplate GetNameplateFromPool()
    {
        if (_nameplatePool.Count > 0) return _nameplatePool.Dequeue();

        var go = Instantiate(UIEntityNameplatePrefab, UICanvas.transform).GetComponent<UIEntityNameplate>();
        return go;
    }

    private void ReturnNameplateToPool(UIEntityNameplate nameplate)
    {
        nameplate.Clear();
        nameplate.gameObject.SetActive(false);
        _nameplatePool.Enqueue(nameplate);
    }
    
    private void UpdateNameplatePosition(UIEntityNameplate nameplate, Entity entity)
    {
        var worldPosition = entity.transform.position;

        var collider = nameplate.GetCachedCollider();
        if (collider is null) return;
        worldPosition.y += collider.bounds.extents.y;

        var screenPosition = GameManager.Instance.PlayerCamera.WorldToScreenPoint(worldPosition);
        nameplate.transform.position = screenPosition;
        nameplate.gameObject.SetActive(screenPosition.z > 0);
    }

    #endregion

    private void Update()
    {
        foreach (var kvp in _activeNameplates)
        {
            var entity = kvp.Key;
            var nameplate = kvp.Value;
            UpdateNameplatePosition(nameplate, entity);
        }
    }
}