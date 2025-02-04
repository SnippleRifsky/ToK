using UnityEngine;

public abstract class BaseXpSystem : ScriptableObject
{
    public virtual void Initialize(Player owner)
    {
        Owner = owner;
        CurrentLevel = 1;
        CurrentXp = 0;
        AtLevelCap = false;
        GetLevelXpRange();
        PublishLevelingUpdate();
    }

    protected abstract void PublishLevelingUpdate();

    public Player Owner { get; private set; }
    
    public int CurrentXp { get; protected set; }
    public int CurrentLevel { get; protected set; }
    protected int MaxLevel { get; set; } = 60;

    public int RemainingXp { get; protected set; } = 0;

    public bool AtLevelCap { get; protected set; }

    public abstract bool AddXp(int amount);
    public abstract int XpToNextLevel();

    public abstract int GetLevelXpRange();
    public abstract bool LevelUp();
}