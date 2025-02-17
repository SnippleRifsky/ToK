using UnityEngine;
using UnityEngine.UI;


public class InventoryTest : MonoBehaviour
{
    private PlayerInventory _playerInventory;

    
    [SerializeField]private Sprite _swordSprite;
    [SerializeField]private Sprite _appleSprite;
    [SerializeField]private Sprite _healthPotionSprite;


    void Start()
    {
        // Initialize the inventory system with a capacity of 10 slots
        _playerInventory = GameManager.Instance.PlayerInventory;

        // Create test items
        InventoryItem sword = ScriptableObject.CreateInstance<InventoryItem>();
        sword.Initialize("sword_001", "Iron Sword", "A simple iron sword.", _swordSprite, false, 1, 1);
        InventoryItem potion = ScriptableObject.CreateInstance<InventoryItem>();
        potion.Initialize("potion_001", "Health Potion", "Restores 50 health points.", _healthPotionSprite, true, 10, 5);
        InventoryItem arrow = ScriptableObject.CreateInstance<InventoryItem>();
        arrow.Initialize("arrow_001", "Arrow", "A simple iron arrow.", _appleSprite, true, 100, 20);

        // Add items to the inventory
        _playerInventory.AddItem(sword);
        _playerInventory.AddItem(potion);
        _playerInventory.AddItem(arrow);

        // Manipulate items in the inventory
        _playerInventory.RemoveItem(1, 2); // Remove 2 potions from slot 1
        _playerInventory.MoveItem(2, 3); // Move arrows from slot 2 to slot 3

        // Log inventory state to verify
        LogInventoryState();
    }

    private void LogInventoryState()
    {
        for (int i = 0; i < _playerInventory.Capacity; i++)
        {
            InventoryItem item = _playerInventory.GetItem(i);
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