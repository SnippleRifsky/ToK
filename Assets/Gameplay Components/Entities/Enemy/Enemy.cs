using UnityEngine;

public class Enemy : Entity, IHealthProvider
{
    [SerializeField] private EnemyConfig enemyConfig;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Color _originalMaterialColor;
    private bool _isDetected;

    public float CurrentHealth => Stats.Resources.CurrentHealth;
    public float MaxHealth => Stats.MaxHealth;

    private int _xpValue;
    
    public void Initialize(EnemyConfig config)
    {
        enemyConfig = config;
        baseStats = config.CreateBaseStats();
        level = config.level;
        entityName = config.entityName;
        _xpValue = config.xpValue;
        Stats = new Stats(new StatsMediator(), baseStats, gameObject);

        // Apply mesh and material if provided
        if (config.enemyMesh != null)
        {
            _meshFilter.mesh = config.enemyMesh;
        }

        if (config.enemyMaterial != null)
        {
            _meshRenderer.material = config.enemyMaterial;
            _originalMaterialColor = config.enemyMaterial.color;
        }
    }

    protected override void Awake()
    {
        if (enemyConfig != null)
        {
            Initialize(enemyConfig);
        }
        base.Awake();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EventBus.Subscribe<EntityEvents.DetectionStatusChanged>(OnDetectionStatusChanged);
    }
    
    protected override void OnDisable()
    {
        base.OnDisable();
        EventBus.Unsubscribe<EntityEvents.DetectionStatusChanged>(OnDetectionStatusChanged);
    }

    private void OnDetectionStatusChanged(EntityEvents.DetectionStatusChanged evt)
    {
        if (evt.Entity != this) return;
        _isDetected = evt.IsDetected;
    }

    public void OnTargeted()
    {
        _originalMaterialColor = _meshRenderer.material.color;
        if (_meshRenderer is null) return;
        _meshRenderer.material.color = Color.red;
        GameManager.Instance.UIManager.NameplateManager.ShowEntityNameplate(this);
    }

    public void OnUntargeted()
    {
        if (_meshRenderer is null) return;
        _meshRenderer.material.color = _originalMaterialColor;
        if (_isDetected) return;
        GameManager.Instance.UIManager.NameplateManager.HideEntityNameplate(this);
    }
}