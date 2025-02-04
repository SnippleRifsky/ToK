using UnityEngine;

public class ResourceSystem
{
    private readonly Stats stats;
    private readonly GameObject owner;
    private float currentHealth;
    private float currentResource;

    public ResourceSystem(Stats stats, GameObject owner)
    {
        this.stats = stats;
        this.owner = owner;
        CurrentHealth = stats.MaxHealth;
        CurrentResource = stats.MaxResource;
    }

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = Mathf.Clamp(value, 0, stats.MaxHealth);
            if (owner.TryGetComponent<IHealthProvider>(out var healthProvider))
            {
                EventBus.Publish(new EntityEvents.HealthChanged(currentHealth, stats.MaxHealth, healthProvider));
            }
        }
    }

    public float CurrentResource
    {
        get => currentResource;
        set
        {
            currentResource = Mathf.Clamp(value, 0, stats.MaxResource);
            if (owner.TryGetComponent<IResourceProvider>(out var resourceProvider))
            { 
                EventBus.Publish(new EntityEvents.ResourceChanged(currentResource, stats.MaxResource, resourceProvider));
            }
        }
    }

    public void Update(float deltaTime)
    {
        if (CurrentHealth < stats.MaxHealth) CurrentHealth += stats.HealthRegen * deltaTime;
        if (CurrentResource < stats.MaxResource) CurrentResource += stats.ResourceRegen * deltaTime;
    }
}