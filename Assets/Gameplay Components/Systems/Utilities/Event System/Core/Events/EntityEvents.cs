public static class EntityEvents
{
    public readonly struct TargetChanged
    {
        public readonly Entity Target;

        public TargetChanged(Entity target)
        {
            Target = target;
        }
    }

    public readonly struct HealthChanged
    {
        public readonly float CurrentHealth;
        public readonly float MaxHealth;
        public readonly IHealthProvider Entity;

        public HealthChanged(float currentHealth, float maxHealth, IHealthProvider entity)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Entity = entity;
        }
    }

    public readonly struct ResourceChanged
    {
        public readonly float CurrentResource;
        public readonly float MaxResource;
        public readonly IResourceProvider Entity;

        public ResourceChanged(float currentResource, float maxResource, IResourceProvider entity)
        {
            CurrentResource = currentResource;
            MaxResource = maxResource;
            Entity = entity;
        }
    }

    public readonly struct DetectionStatusChanged
    {
        public readonly Entity Entity;
        public readonly bool IsDetected;
        public readonly Entity Source;

        public DetectionStatusChanged(Entity entity, bool isDetected, Entity source)
        {
            Entity = entity;
            IsDetected = isDetected;
            Source = source;
        }
    }

    public readonly struct EntityDeathEvent
    {
        public readonly Entity Entity;

        public EntityDeathEvent(Entity entity)
        {
            Entity = entity;
        }
    }
}