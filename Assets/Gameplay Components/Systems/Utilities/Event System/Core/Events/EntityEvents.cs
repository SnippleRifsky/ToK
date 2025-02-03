public static class EntityEvents
{
    public readonly struct TargetChanged
    {
        public readonly Entity Target;
        public TargetChanged(Entity target) => Target = target;
    }
    
    public readonly struct HealthChanged
    {
        public readonly float CurrentHealth;
        public readonly float MaxHealth;
        public readonly Entity Entity;
        public HealthChanged(float currentHealth, float maxHealth, Entity entity)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            Entity = entity;
        }
    }
}