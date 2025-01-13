using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

[CreateAssetMenu(fileName = "LinearXpSystem", menuName = "RPG Components/ Linear Xp System")]
public class LinearXpSystem : BaseXpSystem
{
    [SerializeField] private float offset = 1f;
    [SerializeField] private float curveGain = 1f;
    private int _levelXpAmount;
    
    public override bool AddXp(int amount)
    {
        if (AtLevelCap)
        {
            Debug.Log($"Failed to add {amount} xp");
            return false;
        }
        
        if ((CurrentXp + amount) >= _levelXpAmount)
        {
            var carryoverXpAmount = (CurrentXp+amount) - _levelXpAmount;
            CurrentXp += _levelXpAmount;
            LevelUp();
            AddXp(carryoverXpAmount);
        }
        else
        {
            CurrentXp += amount;
            Debug.Log($"Added {amount} xp");
        }
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
        _levelXpAmount = (int)Mathf.Round(Mathf.Pow((CurrentLevel/curveGain), offset));
        Debug.Log($"Xp required for {CurrentLevel}: {_levelXpAmount}");
        return _levelXpAmount;
    }
}
