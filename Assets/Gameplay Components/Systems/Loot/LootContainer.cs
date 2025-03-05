using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LootContainer : MonoBehaviour, ILootable
{
    [Serialize] private IInventorySystem _inventory;
    [SerializeField] private LootConfig _config;
    
    public void Awake()
    {
        EventBus.Subscribe<LootEvents.LootRequested>(OnLootRequested);
        _config?.Initialize();
        _inventory = _config?.GetInventory();
    }

    public void OnLootRequested(LootEvents.LootRequested evt)
    {
        if (evt.Source != this) return;
        if (!_inventory.IsEmpty()) return;
        _config?.GenerateLoot(_inventory);
    }
    
    public void OnDestroy()
    {
        EventBus.Unsubscribe<LootEvents.LootRequested>(OnLootRequested);
    }
    
    public IInventorySystem GetInventory()
    {
        return _inventory;
    }
}