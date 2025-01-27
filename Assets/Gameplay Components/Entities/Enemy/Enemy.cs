using UnityEngine;

public class Enemy : Entity
{
    private UIEntityNameplate _entityNameplate;
    private MeshRenderer _meshRenderer;
    private Material _originalMaterial;

    protected override void Awake()
    {
        base.Awake();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (_meshRenderer != null)
        {
            // Store the original material
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
            // You might want to create a highlighted material in your project
            // and assign it here instead of just changing the color
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