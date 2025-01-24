using System;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    private PlayerData _playerData;
    private Slider _healthBar;
    private Slider _resourceBar;

    private void Awake()
    {
        _healthBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Health Bar");
        _resourceBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Resource Bar");
        
        _playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        if (_playerData is not null)
        {
            _playerData.Stats.Resources.OnHealthChanged += UpdateHealthBar;
            _playerData.Stats.Resources.OnResourceChanged += UpdateResourceBar;
        }
        else
        {
            Debug.LogWarning("No player data found");
        }
    }

    private void OnDestroy()
    {
        _playerData.Stats.Resources.OnHealthChanged -= UpdateHealthBar;
        _playerData.Stats.Resources.OnResourceChanged -= UpdateResourceBar;
    }
    
    private void UpdateHealthBar(float newHealth)
    {
        _healthBar.value = (newHealth / _playerData.Stats.MaxHealth);
    }

    private void UpdateResourceBar(float newResource)
    {
        _resourceBar.value = (newResource / _playerData.Stats.MaxResource);
    }
}
