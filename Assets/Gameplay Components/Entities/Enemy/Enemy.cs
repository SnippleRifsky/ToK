using UnityEngine;

public class Enemy : Entity, IHealthProvider
{
    private MeshRenderer _meshRenderer;
    private Color _originalMaterialColor;
    private bool _isDetected;

    public float CurrentHealth => Stats.Resources.CurrentHealth;
    public float MaxHealth => Stats.MaxHealth;

    protected override void Awake()
    {
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