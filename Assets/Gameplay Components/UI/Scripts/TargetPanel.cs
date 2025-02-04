using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetPanel : MonoBehaviour
{
    private Slider _healthBar;
    private IHealthProvider _currentTarget;
    private Player _player;
    private TextMeshProUGUI _entityLevelText;
    private TextMeshProUGUI _entityNameText;

    private void Awake()
    {
        _healthBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Health Bar");
        _player = GameManager.Instance.Player;
        if (_player is not null)
        {
            EventBus.Subscribe<EntityEvents.HealthChanged>(OnHealthChanged);
            EventBus.Subscribe<EntityEvents.TargetChanged>(OnTargetChanged);
        }

        var textComponents = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textComponent in textComponents)
            switch (textComponent.name)
            {
                case "EntityLevel":
                    _entityLevelText = textComponent;
                    break;
                case "EntityName":
                    _entityNameText = textComponent;
                    break;
            }

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<EntityEvents.HealthChanged>(OnHealthChanged);
    }

    private void UpdateHealthBar(float newHealth)
    {
        _healthBar.value = newHealth / _currentTarget.MaxHealth;
    }

    private void OnHealthChanged(EntityEvents.HealthChanged evt)
    {
        if (!ReferenceEquals(evt.Entity, _currentTarget)) return;
        UpdateHealthBar(evt.CurrentHealth);
    }

    private void OnTargetChanged(EntityEvents.TargetChanged evt)
    {
        _currentTarget = evt.Target as IHealthProvider;

        if (_currentTarget is null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            UpdateHealthBar(_currentTarget.CurrentHealth);
            gameObject.SetActive(true);
        }

        if (evt.Target is not null)
        {
            _entityNameText.text = evt.Target.EntityName;
            _entityLevelText.text = evt.Target.Level.ToString();
        }
        else
        {
            _entityNameText.text = string.Empty;
            _entityLevelText.text = string.Empty;
        }
        
    }
}