using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "NewLootConfig", menuName = "RPG Components/Systems/Loot/Configs/Loot Config")]
public class LootConfig : ScriptableObject
{
    public int Capacity { get; private set; }
    [SerializeField] private int MinItems;
    [SerializeField] private int MaxItems;
    [SerializeField] IInventorySystem inventory;
    [SerializeField] private LootTable lootTable;
    
    public LootTable LootTable => lootTable;
    public IInventorySystem Inventory => inventory;

    public void Initialize()
    {
        Capacity = Random.Range(MinItems, MaxItems);
        inventory = new LootInventory(this);
    }

    public void GenerateLoot(IInventorySystem invSystem)
    {
        if (invSystem == null)
        {
            Debug.LogError("Inventory system is null");
            return;
        }
        inventory = invSystem;
        for (var i = 0; i < Capacity; i++)
        {
            var item = lootTable?.GetRandomItem();
            if (item is null) continue;
            inventory.AddItem(item); 
        }
    }
    
    public IInventorySystem GetInventory()
    {
        return inventory;
    }
}