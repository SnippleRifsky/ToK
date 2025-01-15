using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;
using UnityEngine.Windows.WebCam;

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
            // Check if this amount will cause a level up
            if (CurrentXp + remainingXp < _levelXpAmount)
            {
                CurrentXp += remainingXp;
                Debug.Log($"Added {remainingXp} xp");
                remainingXp = 0;
            }
            else
            {
                // Calculate how much XP carries over after level up
                var xpToLevel = _levelXpAmount - CurrentXp;
                remainingXp -= xpToLevel;
                CurrentXp = 0;
                LevelUp();
            }
        }

        if (remainingXp > 0 && AtLevelCap)
        {
            Debug.Log($"Couldn't add {remainingXp} xp - at level cap");
        }

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
        _levelXpAmount = (int)Mathf.Round(Mathf.Pow((CurrentLevel/curveGain), offset));
        Debug.Log($"Xp required for {CurrentLevel}: {_levelXpAmount}");
        return _levelXpAmount;
    }
}
