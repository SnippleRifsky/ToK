using System.Collections.Generic;
using UnityEngine;

public class NameplateManager : MonoBehaviour
{
    private Canvas _uiCanvas;

    [SerializeField] private GameObject _entityNameplatePrefab;

    private readonly Queue<UIEntityNameplate> _nameplatePool = new();
    private readonly Dictionary<Entity, UIEntityNameplate> _activeNameplates = new();

    private const float MIN_SCREEN_DEPTH = 0f;

    #region Initialization

    public void Initialize()
    {
        _uiCanvas = UIManager.Instance.UICanvas;
        EventBus.Subscribe<EntityEvents.EntityDeathEvent>(OnEntityDestroyed);
    }

    #endregion

    #region Nameplate Management

    public void ShowEntityNameplate(Entity entity)
    {
        if (_activeNameplates.ContainsKey(entity)) return;

        var nameplate = GetNameplateFromPool();
        nameplate.Setup(entity);
        UpdateNameplatePosition(nameplate, entity);
        _activeNameplates[entity] = nameplate;

        if (entity is IHealthProvider healthProvider)
            EventBus.Publish(new EntityEvents.HealthChanged(
                healthProvider.CurrentHealth,
                healthProvider.MaxHealth,
                healthProvider
            ));
    }

    public void HideEntityNameplate(Entity entity)
    {
        if (_activeNameplates.TryGetValue(entity, out var nameplate))
        {
            ReturnNameplateToPool(nameplate);
            _activeNameplates.Remove(entity);
        }
    }

    #endregion

    #region Helper Methods

    private UIEntityNameplate GetNameplateFromPool()
    {
        if (_nameplatePool.Count > 0) return _nameplatePool.Dequeue();

        var newNameplate = Instantiate(_entityNameplatePrefab, _uiCanvas.transform)
            .GetComponent<UIEntityNameplate>();
        return newNameplate;
    }

    private void ReturnNameplateToPool(UIEntityNameplate nameplate)
    {
        nameplate.Clear();
        nameplate.gameObject.SetActive(false);
        _nameplatePool.Enqueue(nameplate);
    }

    private void UpdateNameplatePosition(UIEntityNameplate nameplate, Entity entity)
    {
        var screenPosition = CalculateScreenPosition(entity, nameplate);
        nameplate.transform.position = screenPosition;
        nameplate.gameObject.SetActive(screenPosition.z > MIN_SCREEN_DEPTH);
    }

    private Vector3 CalculateScreenPosition(Entity entity, UIEntityNameplate nameplate)
    {
        var worldPosition = entity.transform.position;

        var entityCollider = nameplate.GetCachedCollider();
        if (entityCollider is not null) worldPosition.y += entityCollider.bounds.extents.y;

        return GameManager.Instance.PlayerCamera.WorldToScreenPoint(worldPosition);
    }

    #endregion

    #region Update Logic

    private void Update()
    {
        UpdateAllNameplatePositions();
    }

    private void UpdateAllNameplatePositions()
    {
        var playerCamera = GameManager.Instance.PlayerCamera;

        foreach (var kvp in _activeNameplates)
        {
            var entity = kvp.Key;
            var nameplate = kvp.Value;
            UpdateNameplatePosition(nameplate, entity);
        }
    }

    #endregion

    private void OnDestroy()
    {
        EventBus.Unsubscribe<EntityEvents.EntityDeathEvent>(OnEntityDestroyed);
    }

    private void OnEntityDestroyed(EntityEvents.EntityDeathEvent evt)
    {
        if (_activeNameplates.TryGetValue(evt.Entity, out var nameplate))
        {
            Destroy(nameplate.gameObject);
            _activeNameplates.Remove(evt.Entity);
        }
    }
}