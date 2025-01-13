using UnityEngine;
public abstract class BaseXpSystem : ScriptableObject
{
    public int CurrentXp { get; protected set; } = 0;
    public int CurrentLevel { get; protected set; } = 1;
    protected int MaxLevel { get; set; } = 60;
    
    public int RemainingXp { get; protected set; } = 0;

    public bool AtLevelCap { get; protected set; } = false;

    public abstract bool AddXp(int amount);
    public abstract int XpToNextLevel();

    public abstract int GetLevelXpRange();
    public abstract bool LevelUp();
}
