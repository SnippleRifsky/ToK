using System;

public interface IHealthProvider
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
}

public interface IResourceProvider : IHealthProvider
{
    float CurrentResource { get; }
    float MaxResource { get; }
}