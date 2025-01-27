using System;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    private IHealthProvider _healthProvider;
    private IResourceProvider _resourceProvider;
    private Slider _healthBar;
    private Slider _resourceBar;
    
    private void Awake()
    {
        _healthBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Health Bar");
        _resourceBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Resource Bar");
    }

    public void Initalize(IHealthProvider provider)
    {
        UnsubscribeFromEvents();

        _healthProvider = provider;
        _resourceProvider = provider as IResourceProvider;
        
        SubscribeToEvents();
        UpdateUi();
    }
    
    private void SubscribeToEvents()
    {
        _healthProvider.OnHealthChanged += UpdateHealthBar;
        if (_resourceProvider != null) _resourceProvider.OnResourceChanged += UpdateResourceBar;
    }
    
    private void UnsubscribeFromEvents()
    {
        if (_healthProvider != null) _healthProvider.OnHealthChanged -= UpdateHealthBar;
        if (_resourceProvider != null) _resourceProvider.OnResourceChanged -= UpdateResourceBar;
    }
    
    private void UpdateUi()
    {
        UpdateHealthBar(_healthProvider.CurrentHealth);
        if (_resourceProvider != null) UpdateResourceBar(_resourceProvider.CurrentResource);
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }
    
    private void UpdateHealthBar(float newHealth)
    {
        _healthBar.value = (newHealth / _healthProvider.MaxHealth);
    }

    private void UpdateResourceBar(float newResource)
    {
        if (_resourceProvider == null) return;
        _resourceBar.value = (newResource / _resourceProvider.MaxResource);
    }
}