using System;
using UnityEngine;

public class ResourceSystem
{
    private readonly Stats stats;
    private float currentHealth;
    private float currentResource;
    public event Action<float> OnHealthChanged;
    public event Action<float> OnResourceChanged;

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, stats.MaxHealth);
            OnHealthChanged?.Invoke(CurrentHealth);
        } 
    }

    public float CurrentResource
    {
        get => currentResource;
        set
        {
            currentResource = Mathf.Clamp(value, 0, stats.MaxResource);
            OnResourceChanged?.Invoke(CurrentResource);
        } 
    }

    public ResourceSystem(Stats stats)
    {
        this.stats = stats;
        CurrentHealth = stats.MaxHealth;
        CurrentResource = stats.MaxResource;
    }

    public void Update(float deltaTime)
    {
        if (CurrentHealth < stats.MaxHealth)
        {
            CurrentHealth += stats.HealthRegen * deltaTime;
        }
        
        if (CurrentResource < stats.MaxResource)
        {
            CurrentResource += stats.ResourceRegen * deltaTime;
        }
    }
}