using UnityEngine;

public enum ResourceType
{
    Mana,
    Rage,
    Energy
}

public abstract class ResourceBehavior
{
    protected readonly Stats stats;
    protected float currentValue;

    protected ResourceBehavior(Stats stats)
    {
        this.stats = stats;
        CurrentValue = stats.MaxResource;
    }

    public float CurrentValue
    {
        get => currentValue;
        set => currentValue = Mathf.Clamp(value, 0, stats.MaxResource);
    }

    public abstract void Update(float deltaTime);
}

public class ManaBehavior : ResourceBehavior
{
    private readonly float regenRate;

    public ManaBehavior(Stats stats, float regenRate) : base(stats)
    {
        this.regenRate = regenRate;
    }

    public override void Update(float deltaTime)
    {
        if (CurrentValue >= stats.MaxResource) CurrentValue += regenRate * deltaTime;
    }
}