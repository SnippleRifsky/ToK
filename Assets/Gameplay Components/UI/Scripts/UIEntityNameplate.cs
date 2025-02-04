using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEntityNameplate : MonoBehaviour
{
    private CapsuleCollider _collider;
    private Entity _entity;
    private Slider _healthBar;
    private TextMeshProUGUI _levelLabel;
    private TextMeshProUGUI _nameLabel;

    public void Setup(Entity entity)
    {
        _entity = entity;
        _levelLabel = GetComponentsInChildren<TextMeshProUGUI>()
            .FirstOrDefault(text => text.gameObject.name == "EntityLevelTag");
        _nameLabel = GetComponentsInChildren<TextMeshProUGUI>()
            .FirstOrDefault(text => text.gameObject.name == "EntityNameTag");
        _healthBar = GetComponent<Slider>();
        if (_entity.Stats is not null)
        {
            EventBus.Subscribe<EntityEvents.HealthChanged>(OnHealthChanged);
        }
        else
            Debug.LogWarning("Failed to bind to OnHealthChanged event");
        _collider = entity.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;
        if (_entity is not null) UpdateUIText();
    }

    public void Clear()
    {
        _entity = null;
        _levelLabel.text = string.Empty;
        _nameLabel.text = string.Empty;
        _collider = null;
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<EntityEvents.HealthChanged>(OnHealthChanged);
    }

    private void OnHealthChanged(EntityEvents.HealthChanged evt)
    {
        if (!gameObject || !gameObject.activeSelf || !ReferenceEquals(evt.Entity, _entity)) return;
        _healthBar.value = evt.CurrentHealth / evt.MaxHealth;
    }

    private void UpdateUIText()
    {
        if (!gameObject.activeSelf) return;
        _levelLabel.text = _entity.Level.ToString();
        _nameLabel.text = _entity.EntityName;
    }

    public CapsuleCollider GetCachedCollider()
    {
        return _collider;
    }
}