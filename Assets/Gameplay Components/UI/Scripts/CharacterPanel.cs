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
            _playerData.OnHealthChanged += UpdateHealthBar;
            _playerData.OnResourceChanged += UpdateResourceBar;
        }
        else
        {
            Debug.LogWarning("No player data found");
        }
    }

    private void OnDestroy()
    {
        _playerData.OnHealthChanged -= UpdateHealthBar;
    }
    
    private void UpdateHealthBar(float newHealth)
    {
        // TODO make health regen trigger event
        _healthBar.value = (newHealth / _playerData.Stats.MaxHealth);
    }

    private void UpdateResourceBar(float newResource)
    {
        // TODO make resource regen trigger event
        _resourceBar.value = (newResource / _playerData.Stats.MaxResource);
    }
}
