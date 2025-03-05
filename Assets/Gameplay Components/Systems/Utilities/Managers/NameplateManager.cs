using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NameplateManager : MonoBehaviour
{
    private Canvas _uiCanvas;

    private readonly string _entityNameplatePrefabPath = "Data/Prefabs/UIEntityNameplate";
    private GameObject _entityNameplatePrefab;

    private ObjectPool<UIEntityNameplate> _nameplatePool;
    private readonly Dictionary<Entity, UIEntityNameplate> _activeNameplates = new();

    #region Initialization

    public void Initialize()
    {
        _entityNameplatePrefab = Resources.Load<GameObject>(_entityNameplatePrefabPath);
        _uiCanvas = UIManager.Instance.UICanvas;

        _nameplatePool = new ObjectPool<UIEntityNameplate>(
            CreateNameplate,
            OnGetNameplate,
            OnReleaseNameplate,
            OnDestroyNameplate,
            false,
            25,
            100
        );

        EventBus.Subscribe<EntityEvents.EntityDeathEvent>(OnEntityDestroyed);
    }

    private UIEntityNameplate CreateNameplate()
    {
        var newNameplate = Instantiate(_entityNameplatePrefab, _uiCanvas.transform).GetComponent<UIEntityNameplate>();
        return newNameplate;
    }

    private void OnGetNameplate(UIEntityNameplate nameplate)
    {
        nameplate.gameObject.SetActive(true);
    }

    private void OnReleaseNameplate(UIEntityNameplate nameplate)
    {
        nameplate.Clear();
        nameplate.gameObject.SetActive(false);
    }

    private void OnDestroyNameplate(UIEntityNameplate nameplate)
    {
        Destroy(nameplate.gameObject);
    }

    #endregion

    #region Nameplate Management

    public void ShowEntityNameplate(Entity entity)
    {
        if (_activeNameplates.ContainsKey(entity)) return;

        var nameplate = _nameplatePool.Get();
        nameplate.Setup(entity);
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
            _nameplatePool.Release(nameplate);
            _activeNameplates.Remove(entity);
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
            _nameplatePool.Release(nameplate);
            _activeNameplates.Remove(evt.Entity);
        }
    }
}