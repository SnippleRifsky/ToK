using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
[CreateAssetMenu(fileName = "NewLootTable", menuName = "RPG Components/Systems/Loot/Tables/Loot Table")]
public class LootTable : ScriptableObject
{
    [SerializeField] private List<LootDrops> _items;
    
    public LootTable()
    {
        _items = new List<LootDrops>();
    }
    
    public InventoryItem GetRandomItem()
    {
        if (_items == null || _items.Count == 0)
        {
            Debug.LogError($"No items configured in {name}");
            return null;
        }

        var totalWeight = 0;
        foreach (var item in _items) totalWeight += item.DropChance;

        var random = Random.Range(0, totalWeight);
        var currentWeight = 0;

        foreach (var item in _items)
        {
            currentWeight += item.DropChance;
            if (random <= currentWeight) return item.Item;
        }

        return _items[0].Item;
    }
}

[Serializable]
public class LootDrops
{
    public InventoryItem Item;
    public int DropChance;
}