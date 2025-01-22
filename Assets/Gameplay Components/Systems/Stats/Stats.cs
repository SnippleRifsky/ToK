using System;
using UnityEngine;

public enum StatType
{
    Attack,
    Defense,
    MaxHealth,
    MaxResource,
    HealthRegen,
    ResourceRegen,
}
public class Stats
{
    internal readonly StatsMediator mediator;
    internal readonly BaseStats baseStats;
    private readonly ResourceSystem resourceSystem;
    
    public StatsMediator Mediator => mediator;
    public ResourceSystem Resources => resourceSystem;

    public void Update(float deltaTime)
    {
        mediator.Update(deltaTime);
        resourceSystem.Update(deltaTime);
    }

    public int Attack
    {
        get
        {
            var query = new Query(StatType.Attack, baseStats.attack);
            mediator.PerformQuery(this, query);
            return query.Value;
        }
    }

    public int Defense
    {
        get
        {
            var query = new Query(StatType.Defense, baseStats.defense);
            mediator.PerformQuery(this, query);
            return query.Value;
        }
    }

    public Stats(StatsMediator mediator, BaseStats baseStats)
    {
        this.mediator = mediator;
        this.baseStats = baseStats;
        resourceSystem = new ResourceSystem(this);
    }
    
    public int MaxHealth
    {
        get
        {
            var query = new Query(StatType.MaxHealth, baseStats.maxHealth);
            mediator.PerformQuery(this, query);
            return query.Value;
        }
    }

    public int MaxResource
    {
        get
        {
            var query = new Query(StatType.MaxResource, baseStats.maxResource);
            mediator.PerformQuery(this, query);
            return query.Value;
        }
    }

    public float HealthRegen
    {
        get
        {
            var query = new Query(StatType.HealthRegen, Mathf.RoundToInt(baseStats.healthRegen * 100));
            mediator.PerformQuery(this, query);
            return query.Value / 100f;
        }
    }

    public float ResourceRegen
    {
        get
        {
            var query = new Query(StatType.ResourceRegen, Mathf.RoundToInt(baseStats.resourceRegen * 100));
            mediator.PerformQuery(this, query);
            return query.Value / 100f;
        }
    }
    
    public override string ToString() => 
        $"Attack: {Attack}, Defense: {Defense}, " +
        $"Health: {Resources.CurrentHealth}/{MaxHealth}, " +
        $"Resource: {Resources.CurrentResource}/{MaxResource}";
}