using System;
using UnityEngine;

public class Enemy : Entity, IHealthProvider
{
    private UIEntityNameplate _entityNameplate;
    private MeshRenderer _meshRenderer;
    private Material _originalMaterial;
    
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
        if (_meshRenderer != null)
        {
            _originalMaterial = _meshRenderer.material;
        }
    }

    private void Start()
    {
        _entityNameplate = gameObject.AddComponent<UIEntityNameplate>();
    }
    
    public void OnTargeted()
    {
        if (_meshRenderer is not null)
        {
            _meshRenderer.material.color = Color.red;
        }
    }

    public void OnUntargeted()
    {
        if (_meshRenderer is not null)
        {
            _meshRenderer.material.color = _originalMaterial.color;
        }
    }
}