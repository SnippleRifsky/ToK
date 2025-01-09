using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class CharacterStat
{
    public float baseValue;
    public float maxValue;
    public string statName;
    public string statDescription;

    public virtual float Value
    {
        get
        {
            if (!IsDirty || !Mathf.Approximately(baseValue, LastBaseValue)) return OriginalValue;
            LastBaseValue = baseValue;
            OriginalValue = CalculateModifiedValue();
            IsDirty = false;
            return OriginalValue;
        }
    }

    protected bool IsDirty = true;
    protected float OriginalValue;
    protected float LastBaseValue = float.MinValue;

    protected readonly List<StatModifier> StatModifiersList;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public CharacterStat()
    {
        StatModifiersList = new List<StatModifier>();
        StatModifiers = StatModifiersList.AsReadOnly();
        statName = string.Empty;
        statDescription = string.Empty;
        maxValue = -1f;
    }

    public CharacterStat(float baseValue)
    {
        this.baseValue = baseValue;
        StatModifiersList = new List<StatModifier>();
        StatModifiers = StatModifiersList.AsReadOnly();
        statName = string.Empty;
        statDescription = string.Empty;
        maxValue = -1f;
    }
    
    public CharacterStat(float baseValue, float maxValue, string statName)
    {
        this.baseValue = baseValue;
        StatModifiersList = new List<StatModifier>();
        StatModifiers = StatModifiersList.AsReadOnly();
        this.statName = statName;
        statDescription = string.Empty;
        this.maxValue = maxValue;
    }
    
    public CharacterStat(float baseValue, string statName, string statDescription)
    {
        this.baseValue = baseValue;
        StatModifiersList = new List<StatModifier>();
        StatModifiers = StatModifiersList.AsReadOnly();
        this.statName = statName;
        this.statDescription = statDescription;
        maxValue = -1f;
    }
    
    public CharacterStat(float baseValue, float maxValue ,string statName, string statDescription)
    {
        this.baseValue = baseValue;
        StatModifiersList = new List<StatModifier>();
        StatModifiers = StatModifiersList.AsReadOnly();
        this.statName = statName;
        this.statDescription = statDescription;
        this.maxValue = maxValue;
    }
    
    

    public virtual void AddModifier(StatModifier modifier)
    {
        IsDirty = true;
        StatModifiersList.Add(modifier);
        StatModifiersList.Sort(CompareModifiers);
    }

    protected virtual int CompareModifiers(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
            return -1;
        if (a.Order > b.Order)
            return 1;
        return 0;
    }

    public virtual bool RemoveModifier(StatModifier modifier)
    {
        if (!StatModifiersList.Remove(modifier)) return false;
        IsDirty = true;
        return true;
    }

    public virtual bool RemoveModifierBySource(object source)
    {
        var removed = false;

        for (var i = StatModifiersList.Count - 1; i >= 0; i--)
        {
            if (StatModifiersList[i].Source != source) continue;
            IsDirty = true;
            removed = true;
            StatModifiersList.RemoveAt(i);
        }

        return removed;
    }

    protected virtual float CalculateModifiedValue()
    {
        var moddedValue = baseValue;
        float sumPercentAdd = 0;

        for (var index = 0; index < StatModifiersList.Count; index++)
        {
            var mod = StatModifiersList[index];
            switch (mod.Type)
            {
                case StatModType.Flat:
                    moddedValue += mod.Value;
                    break;
                case StatModType.PercentMult:
                    moddedValue *= 1 + mod.Value;
                    break;
                case StatModType.PercentAdd:
                {
                    sumPercentAdd += mod.Value;
                    if (index + 1 < StatModifiersList.Count &&
                        StatModifiersList[index].Type + 1 == StatModType.PercentAdd) continue;
                    moddedValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                    break;
                }
            }
        }

        if (maxValue > 0 || moddedValue > maxValue)
        {
            return maxValue;
        }
        else
        {
            return (float)Math.Round(moddedValue, 4);
        }
    }
}