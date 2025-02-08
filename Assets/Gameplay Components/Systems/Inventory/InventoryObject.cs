using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "RPG Components/Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();

    public void AddItem(ItemObject item, int amount)
    {
        bool found = false;
        foreach (var slot in Container.Where(t => t.Item == item))
        {
            slot.AddToStack(amount);
            found = true;
            break;
        }
        
        if (!found) Container.Add(new InventorySlot(item, amount));
    }
}