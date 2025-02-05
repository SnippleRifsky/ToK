using UnityEngine;

public class Enemy : Entity, IHealthProvider, IXpProvider
{
    [SerializeField] private EnemyConfig enemyConfig;
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private Color _originalMaterialColor;
    private bool _isDetected;

    public float CurrentHealth => Stats.Resources.CurrentHealth;
    public float MaxHealth => Stats.MaxHealth;

    public int XpValue { get; private set; }

    public void Initialize(EnemyConfig config)
    {
        enemyConfig = config;
        baseStats = config.CreateBaseStats();
        level = config.level;
        entityName = config.entityName;
        XpValue = config.xpValue;
        Stats = new Stats(new StatsMediator(), baseStats, gameObject);
        base.Awake();

        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (_meshRenderer is null)
        {
            // Create a mesh renderer if one doesn't exist
            var meshObject = new GameObject("Mesh");
            meshObject.transform.SetParent(transform);
            meshObject.transform.localPosition = Vector3.zero;
            _meshRenderer = meshObject.AddComponent<MeshRenderer>();
            _meshFilter = meshObject.AddComponent<MeshFilter>();
        }
        else
        {
            _meshFilter = _meshRenderer.GetComponent<MeshFilter>();
        }

        // Now call Awake manually since we're initializing after creation
        base.Awake();

        // Configure mesh and material
        if (config.enemyMesh is not null) _meshFilter.mesh = config.enemyMesh;

        if (config.enemyMaterial is not null)
        {
            _meshRenderer.material = config.enemyMaterial;
            _originalMaterialColor = config.enemyMaterial.color;
        }
    }

    protected override void Awake()
    {
        if (enemyConfig is not null) Initialize(enemyConfig);
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

    public override void Die()
    {
        if (_isDying) return;

        if (_lastDamageSource is Player player)
            EventBus.Publish(new PlayerEvents.ExperienceGained(XpValue, this, player));

        base.Die();
    }
}