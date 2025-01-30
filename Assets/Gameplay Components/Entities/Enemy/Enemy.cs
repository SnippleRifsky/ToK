using System;
using UnityEngine;

public class Enemy : Entity, IHealthProvider
{
    private MeshRenderer _meshRenderer;
    private Color _originalMaterialColor;

    public float CurrentHealth => Stats.Resources.CurrentHealth;
    public float MaxHealth => Stats.MaxHealth;

    public event Action<float> OnHealthChanged
    {
        add => Stats.Resources.OnHealthChanged += value;
        remove => Stats.Resources.OnHealthChanged -= value;
    }

    protected override void Awake()
    {
        base.Awake();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public void OnTargeted()
    {
        _originalMaterialColor = _meshRenderer.material.color;
        if (_meshRenderer is null) return;
        _meshRenderer.material.color = Color.red;
        GameManager.Instance.UIManager.ShowEntityNameplate(this);
    }

    public void OnUntargeted()
    {
        if (_meshRenderer is null) return;
        _meshRenderer.material.color = _originalMaterialColor;
        GameManager.Instance.UIManager.HideEntityNameplate(this);
    }
}