using UnityEngine;

[CreateAssetMenu(fileName = "LinearXpSystem", menuName = "RPG Components/ Linear Xp System")]
public class LinearXpSystem : BaseXpSystem
{
    [SerializeField] private float offset = 2f;
    [SerializeField] private float curveGain = 0.095f;
    private int _levelXpAmount;

    public override bool AddXp(int amount)
    {
        var remainingXp = amount;

        while (remainingXp > 0 && !AtLevelCap)
        {
            var xpToNextLevel = XpToNextLevel();
            // Check if amount is enough to level up
            if (remainingXp >= xpToNextLevel)
            {
                remainingXp -= xpToNextLevel;
                CurrentXp += xpToNextLevel;
                LevelUp();
            }
            else
            {
                CurrentXp += remainingXp;
                remainingXp = 0;
            }
        }

        if (remainingXp > 0 && AtLevelCap) Debug.Log($"Couldn't add {remainingXp} xp - at level cap");
        PublishLevelingUpdate();

        return amount != remainingXp;
    }

    public override int XpToNextLevel()
    {
        return RemainingXp = _levelXpAmount - CurrentXp;
    }

    public override bool LevelUp()
    {
        if (CurrentLevel >= MaxLevel)
        {
            AtLevelCap = true;
            return false;
        }

        CurrentLevel++;
        GetLevelXpRange();
        PublishLevelingUpdate();
        return true;
    }

    public override int GetLevelXpRange()
    {
        _levelXpAmount = (int)Mathf.Round(Mathf.Pow(CurrentLevel / curveGain, offset));
        return _levelXpAmount;
    }
    
    public override void Initialize(Player owner)
    {
        base.Initialize(owner);
        GetLevelXpRange();
        PublishLevelingUpdate();
    }
    
    protected override void PublishLevelingUpdate()
    {
        EventBus.Publish(new PlayerEvents.LevelingChanged(
            CurrentLevel,
            CurrentXp,
            XpToNextLevel(),
            AtLevelCap,
            Owner));
    }
}