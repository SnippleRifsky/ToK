using System;

public abstract class StatModifier : IDisposable
{
    private readonly CountdownTimer timer;

    protected StatModifier(float duration)
    {
        // if duration is <= 0, then the duration is permanent
        if (duration <= 0) return;

        timer = new CountdownTimer(duration);
        timer.OnTimerStop += () => MarkedForRemoval = true;
        timer.Start();
    }

    public bool MarkedForRemoval { get; private set; }

    public void Dispose()
    {
        OnDispose.Invoke(this);
    }

    public event Action<StatModifier> OnDispose = delegate { };

    public void Update(float deltaTime)
    {
        timer?.Tick(deltaTime);
    }

    public abstract void Handle(object sender, Query query);
}

public class BaseStatModifier : StatModifier
{
    private readonly Func<int, int> operation;
    private readonly StatType statType;


    public BaseStatModifier(StatType statType, float duration, Func<int, int> operation) : base(duration)
    {
        this.statType = statType;
        this.operation = operation;
    }

    public override void Handle(object sender, Query query)
    {
        if (query.StatType == statType) query.Value += operation(query.Value);
    }
}