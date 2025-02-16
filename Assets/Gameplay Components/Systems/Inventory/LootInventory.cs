using System.Collections.Generic;

public class LootInventory : IInventorySystem
{
    private readonly Dictionary<int, InventoryItem> _items;
    private int _capacity;
    
    
    public int Capacity
    {
        get => _capacity;
        set
        {
            if (value < 0) return;

            var oldCapacity = _capacity;
            _capacity = value;

            if (_capacity < oldCapacity)
                for (var i = _capacity; i < oldCapacity; i++)
                {
                    if (!_items.ContainsKey(i)) continue;
                    _items.Remove(i);
                    EventBus.Publish(new InventoryEvents.ItemRemoved(this, i));
                }

            EventBus.Publish(new InventoryEvents.CapacityChanged(_capacity, oldCapacity));
        }
    }

    public LootInventory()
    {
        _items = new Dictionary<int, InventoryItem>();
        _capacity = 8;
    }
    public bool AddItem(InventoryItem item, int slotIndex = -1)
    {
        // Find the first available slot if no slotIndex is provided
        if (slotIndex == -1)
            slotIndex = FindAvailableSlot();
        
        // Validate the slot index
        if (slotIndex < 0 || slotIndex >= _capacity || _items.ContainsKey(slotIndex))
            return false;
        
        // Add the item to the inventory
        _items[slotIndex] = item.Clone();
        EventBus.Publish(new InventoryEvents.ItemAdded(this, item, slotIndex));
        return true;
    }

    public bool RemoveItem(int slotIndex, int quantity = 1)
    {
        if (!_items.TryGetValue(slotIndex, out var item)) return false;
        if (item.Quantity <= quantity)
        {
            _items.Remove(slotIndex);
            EventBus.Publish(new InventoryEvents.ItemRemoved(this, slotIndex));
        }
        else
        {
            item.SetQuantity(item.Quantity - quantity);
            EventBus.Publish(new InventoryEvents.ItemAdded(this, item, slotIndex));
        }

        return true;
    }

    public InventoryItem GetItem(int slotIndex)
    {
        return _items.TryGetValue(slotIndex, out var item) ? item : null;
    }

    public void Clear()
    {
        var slots = new List<int>(_items.Keys);
        foreach (var slot in slots) RemoveItem(slot, int.MaxValue);
    }
    
    private int FindAvailableSlot()
    {
        for (var i = 0; i < _capacity; i++)
            if (!_items.ContainsKey(i))
                return i;
        return -1; // No available slot found
    }
}