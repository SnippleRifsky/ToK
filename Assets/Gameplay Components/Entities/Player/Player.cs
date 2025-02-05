using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Entity, IResourceProvider
{
    private Camera _mainCamera;
    private int _entityLayer;
    private Enemy _currentTarget;

    public Entity CurrentTarget => _currentTarget;

    public PlayerController PlayerController { get; private set; }
    public CameraController CameraController { get; private set; }
    public CharacterLeveling CharacterLeveling { get; private set; }

    [SerializeField] private float viewRange = 10f;

    private HashSet<Entity> _entitiesInRange = new();
    private HashSet<Entity> _previousEntitiesInRange = new();

    protected override void Awake()
    {
        SetupPlayer();
        Stats = new Stats(new StatsMediator(), baseStats, gameObject);
        _mainCamera = GameManager.Instance.PlayerCamera;
        _entityLayer = LayerMask.GetMask("Entity");
    }

    private void SetupPlayer()
    {
        PlayerController = gameObject.AddComponent<PlayerController>();
        CharacterLeveling = gameObject.AddComponent<CharacterLeveling>();

        for (var i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).gameObject;
            if (child.name != "CameraRig") continue;
            CameraController = child.AddComponent<CameraController>();
            break;
        }

        EventBus.Subscribe<EntityEvents.EntityDeathEvent>(OnEntityDestroyed);
    }

    protected override void Update()
    {
        base.Update();
        UpdateTargeting();
        if (Input.GetKeyDown(KeyCode.Alpha1)) Attack();

        (_previousEntitiesInRange, _entitiesInRange) = (_entitiesInRange, _previousEntitiesInRange);
        _entitiesInRange.Clear();

        foreach (var entity in FindEntitiesInRange())
        {
            _entitiesInRange.Add(entity);
            if (!_previousEntitiesInRange.Contains(entity))
            {
                EventBus.Publish(new EntityEvents.DetectionStatusChanged(entity, true, this));
                UIManager.Instance.NameplateManager.ShowEntityNameplate(entity);
            }
        }

        foreach (var entity in _previousEntitiesInRange.Where(entity => !_entitiesInRange.Contains(entity)))
        {
            EventBus.Publish(new EntityEvents.DetectionStatusChanged(entity, false, this));
            UIManager.Instance.NameplateManager.HideEntityNameplate(entity);
        }
    }

    #region Heath and Resource

    public float CurrentHealth => Stats.Resources.CurrentHealth;
    public float MaxHealth => Stats.MaxHealth;

    public float CurrentResource => Stats.Resources.CurrentResource;
    public float MaxResource => Stats.MaxResource;

    public void SpendResource(float amount)
    {
        Stats.Resources.CurrentResource -= amount;
    }

    #endregion

    public void AddXp(int amount)
    {
        CharacterLeveling.AddXp(amount);
    }

    #region Targeting

    private void UpdateTargeting()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (CursorRaycastService.Instance.TryGetEntityUnderCursor(out var hitEntity))
            switch (hitEntity)
            {
                case Enemy enemy:
                    SetTarget(enemy);
                    break;
                default:
                    SetTarget(null);
                    break;
            }
        else
            SetTarget(null);
    }

    private void SetTarget(Enemy newTarget)
    {
        if (_currentTarget is { } previousTarget) previousTarget.OnUntargeted();

        _currentTarget = newTarget;

        newTarget?.OnTargeted();
        EventBus.Publish(new EntityEvents.TargetChanged(newTarget));
    }

    private void OnEntityDestroyed(EntityEvents.EntityDeathEvent evt)
    {
        if (evt.Entity == _currentTarget) SetTarget(null);
    }

    #endregion

    private void Attack()
    {
        _currentTarget?.TakeDamage(Stats.Attack, this);
    }

    public IEnumerable<Entity> FindEntitiesInRange()
    {
        var entities = new List<Entity>();
        var hits = Physics.OverlapSphere(transform.position, viewRange, _entityLayer);
        foreach (var hit in hits)
            if (hit.TryGetComponent(out Entity entity))
                entities.Add(entity);

        return entities;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<EntityEvents.EntityDeathEvent>(OnEntityDestroyed);
    }
}