using System;
using System.Collections.Generic;

public class CharacterStat
{
    public float BaseValue;

    public float Value {
        get
        {
            if (!_isDirty) return _originalValue;
            _originalValue = CalculateModifiedValue();
            _isDirty = false;
            return _originalValue;
        }
    }

    private bool _isDirty = true;
    private float _originalValue;

    private readonly List<StatModifier> _statModifiers;

    public CharacterStat(float baseValue)
    {
        BaseValue = baseValue;
        _statModifiers = new List<StatModifier>();
    }

    public void AddModifier(StatModifier modifier)
    {
        _isDirty = true;
        _statModifiers.Add(modifier);
        _statModifiers.Sort(CompareModifiers);
    }

    private static int CompareModifiers(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order) 
            return -1;
        else if (a.Order > b.Order)
            return 1;
        return 0;
    }

    public void RemoveModifier(StatModifier modifier)
    {
        _isDirty = true;
        _statModifiers.Remove(modifier);
    }

    private float CalculateModifiedValue()
    {
        float moddedValue = BaseValue;

        foreach (var mod in _statModifiers)
        {
            switch (mod.Type)
            {
                case StatModType.Flat:
                    moddedValue += mod.Value;
                    break;
                case StatModType.Percent:
                    moddedValue *= 1 + mod.Value;
                    break;
            }
        }
        return (float)Math.Round(moddedValue, 4);
    }
}
