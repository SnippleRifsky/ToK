using System;

public interface IHealthProvider
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    event Action<float> OnHealthChanged;
}

public interface IResourceProvider : IHealthProvider
{
    float CurrentResource { get; }
    float MaxResource { get; }
    event Action<float> OnResourceChanged;
}