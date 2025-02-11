using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    private InventorySystem _inventorySystem;

    void Start()
    {
        // Initialize the inventory system with a capacity of 10 slots
        _inventorySystem = GameManager.Instance.InventorySystem;

        // Create test items
        InventoryItem sword = new InventoryItem("sword_001", "Sword", "A sharp blade.", null, false, 1);
        InventoryItem potion = new InventoryItem("potion_001", "Health Potion", "Restores health.", null, true, 10, 8);
        InventoryItem arrow = new InventoryItem("arrow_001", "Arrow", "Ammunition for bows.", null, true, 50, 50);

        // Add items to the inventory
        _inventorySystem.AddItem(sword);
        _inventorySystem.AddItem(potion);
        _inventorySystem.AddItem(arrow);

        // Manipulate items in the inventory
        _inventorySystem.RemoveItem(1, 2); // Remove 2 potions from slot 1
        _inventorySystem.MoveItem(2, 3); // Move arrows from slot 2 to slot 3

        // Log inventory state to verify
        LogInventoryState();
    }

    private void LogInventoryState()
    {
        for (int i = 0; i < _inventorySystem.Capacity; i++)
        {
            InventoryItem item = _inventorySystem.GetItem(i);
            if (item != null)
            {
                Debug.Log($"Slot {i}: {item.Name}, Quantity: {item.Quantity}");
            }
            else
            {
                Debug.Log($"Slot {i}: Empty");
            }
        }
    }
}