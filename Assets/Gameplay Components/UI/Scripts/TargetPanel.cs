﻿using System.Linq;
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
        if (_player is not null) _player.OnTargetChanged += HandleTargetChanged;

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

    private void HandleTargetChanged(Entity newTarget)
    {
        if (_currentTarget != null) _currentTarget.OnHealthChanged -= UpdateHealthBar;

        switch (newTarget)
        {
            case null:
                gameObject.SetActive(false);
                return;
            case IHealthProvider healthProvider:
                _currentTarget = healthProvider;
                _currentTarget.OnHealthChanged += UpdateHealthBar;

                UpdateHealthBar(_currentTarget.CurrentHealth);
                gameObject.SetActive(true);
                break;
        }

        _entityNameText.text = newTarget.EntityName;
        if (newTarget is Enemy enemy)
            _entityLevelText.text = enemy.Level.ToString();
        else
            _entityLevelText.text = string.Empty;
    }

    private void OnDestroy()
    {
        _currentTarget.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float newHealth)
    {
        _healthBar.value = newHealth / _currentTarget.MaxHealth;
    }
}