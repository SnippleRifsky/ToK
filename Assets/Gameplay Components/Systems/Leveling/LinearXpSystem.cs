using UnityEngine;

[CreateAssetMenu(fileName = "LinearXpSystem", menuName = "RPG Components/ Linear Xp System")]
public class LinearXpSystem : BaseXpSystem
{
    [SerializeField] private float offset = 1f;
    [SerializeField] private float curveGain = 1f;
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
            Debug.Log("Unable to level up. Max level reached.");
            return false;
        }

        CurrentLevel++;
        GetLevelXpRange();
        Debug.Log($"Level Up! New Level: {CurrentLevel}");
        return true;
    }

    public override int GetLevelXpRange()
    {
        _levelXpAmount = (int)Mathf.Round(Mathf.Pow(CurrentLevel / curveGain, offset));
        Debug.Log($"Xp required for {CurrentLevel}: {_levelXpAmount}");
        return _levelXpAmount;
    }
}