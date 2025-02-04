using UnityEngine;

public enum StatType
{
    Attack,
    Defense,
    MaxHealth,
    MaxResource,
    HealthRegen,
    ResourceRegen
}

public class Stats
{
    internal readonly BaseStats baseStats;
    internal readonly StatsMediator mediator;
    internal readonly GameObject owner;

    public Stats(StatsMediator mediator, BaseStats baseStats, GameObject owner)
    {
        this.mediator = mediator;
        this.baseStats = baseStats;
        this.owner = owner;
        Resources = new ResourceSystem(this, owner);
    }

    public StatsMediator Mediator => mediator;
    public ResourceSystem Resources { get; }

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

    public void Update(float deltaTime)
    {
        mediator.Update(deltaTime);
        Resources.Update(deltaTime);
    }

    public override string ToString()
    {
        return $"Attack: {Attack}, Defense: {Defense}, " +
               $"Health: {Resources.CurrentHealth}/{MaxHealth}, " +
               $"Resource: {Resources.CurrentResource}/{MaxResource}";
    }
}