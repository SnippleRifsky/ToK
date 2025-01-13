using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

[CreateAssetMenu(fileName = "LinearXpSystem", menuName = "RPG Components/ Linear Xp System")]
public class LinearXpSystem : BaseXpSystem
{
    [SerializeField] private int offset = 250;
    [SerializeField] private float curveGain = 2.64f;
    private int _levelXpAmount;
    
    public override bool AddXp(int amount)
    {
        if (AtLevelCap)
        {
            Debug.Log($"Failed to add {amount} xp");
            return false;
        }
        CurrentXp += amount;
        if (CurrentXp >= _levelXpAmount)
        {
            if (CurrentXp > _levelXpAmount)
            {
                var carryoverXpAmount = CurrentXp - _levelXpAmount;
                LevelUp();
                AddXp(carryoverXpAmount);
                return false;
            }
            LevelUp();
        }
        Debug.Log($"Added {amount} xp");
        return true;
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
        _levelXpAmount = (int)Mathf.Round((CurrentLevel * _levelXpAmount) + offset * curveGain);
        Debug.Log($"Xp required for {CurrentLevel}: {_levelXpAmount}");
        return _levelXpAmount;
    }
}
