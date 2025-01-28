using System;
using UnityEngine;

public class Player : Entity, IResourceProvider
{
    private Camera _mainCamera;
    private int _entityLayer;
    private Enemy _currentTarget;
    
    public event Action<Entity> OnTargetChanged;
    public Entity CurrentTarget => _currentTarget;
    
    public PlayerController PlayerController { get; private set; }
    public CameraController CameraController { get; private set; }
    public CharacterLeveling CharacterLeveling { get; private set; }

    protected override void Awake()
    {
        SetupPlayer();
        Stats = new Stats(new StatsMediator(), baseStats);
        _mainCamera = GetComponentInChildren<Camera>();
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
    }

    protected override void Update()
    {
        base.Update();
        UpdateTargeting();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Attack();
        }
    }

    #region Heath and Resource

    public float CurrentHealth => Stats.Resources.CurrentHealth;
    public float MaxHealth => Stats.MaxHealth;
    public event Action<float> OnHealthChanged
    {
        add => Stats.Resources.OnHealthChanged += value;
        remove => Stats.Resources.OnHealthChanged -= value;
    }
        
    public float CurrentResource => Stats.Resources.CurrentResource;
    public float MaxResource => Stats.MaxResource;
    public event Action<float> OnResourceChanged
    {
        add => Stats.Resources.OnResourceChanged += value;
        remove => Stats.Resources.OnResourceChanged -= value;
    }
    
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
        if (Input.GetMouseButtonDown(0))
        {
            if (CursorRaycastService.Instance.TryGetEntityUnderCursor(out var hitEntity))
            {
                switch (hitEntity)
                {
                    case Enemy enemy:
                        SetTarget(enemy);
                        break;
                    default:
                        SetTarget(null);
                        break;
                }
            }
            else
            {
                SetTarget(null);
            }
        }
    }

    private void SetTarget(Enemy newTarget)
    {
        if (_currentTarget is { } previousTarget)
        {
            previousTarget.OnUntargeted();
        }

        _currentTarget = newTarget;
        
        newTarget?.OnTargeted();
        OnTargetChanged?.Invoke(_currentTarget);
    }

    #endregion
    
    private void Attack()
    {
        _currentTarget?.TakeDamage(Stats.Attack);
    }
}