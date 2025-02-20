using System.Collections.Generic;

public class LootInventory : IInventorySystem
{
    private readonly Dictionary<int, InventoryItem> _items;
    private int _capacity;
    private readonly LootConfig _config;

    public LootInventory(LootConfig config)
    {
        _config = config;
        _items = new Dictionary<int, InventoryItem>();
        _capacity = _config.Capacity;
    }

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

            EventBus.Publish(new InventoryEvents.CapacityChanged(this, _capacity, oldCapacity));
        }
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

    public bool MoveItem(IInventorySystem owner, int fromSlot, int toSlot)
    {
        if (owner != this) return false;
        //TODO prevent item movement when the inventory is not this instance of IInventorySystem
        if (!_items.TryGetValue(fromSlot, out var fromItem)) return false;
        if (toSlot < 0 || toSlot >= _capacity) return false;

        // If the destination slot has an item
        if (_items.TryGetValue(toSlot, out var toItem))
        {
            // If items can be stacked
            if (fromItem.CanStackWith(toItem))
            {
                var totalQuantity = fromItem.Quantity + toItem.Quantity;
                if (totalQuantity <= toItem.MaxStackSize)
                {
                    // Combine stacks
                    toItem.SetQuantity(totalQuantity);
                    _items.Remove(fromSlot);
                    EventBus.Publish(new InventoryEvents.ItemRemoved(this, fromSlot));
                    EventBus.Publish(new InventoryEvents.ItemAdded(this, toItem, toSlot));
                }
                else
                {
                    // Fill the destination stack and leave remaining in source
                    var remainingQuantity = totalQuantity - toItem.MaxStackSize;
                    toItem.SetQuantity(toItem.MaxStackSize);
                    fromItem.SetQuantity(remainingQuantity);
                    EventBus.Publish(new InventoryEvents.ItemAdded(this, toItem, toSlot));
                    EventBus.Publish(new InventoryEvents.ItemAdded(this, fromItem, fromSlot));
                }

                return true;
            }

            // Swap items
            _items[fromSlot] = toItem;
            _items[toSlot] = fromItem;
            EventBus.Publish(new InventoryEvents.ItemAdded(this, toItem, fromSlot));
            EventBus.Publish(new InventoryEvents.ItemAdded(this, fromItem, toSlot));
            return true;
        }

        // Move item to empty slot
        _items.Remove(fromSlot);
        _items[toSlot] = fromItem;
        EventBus.Publish(new InventoryEvents.ItemRemoved(this, fromSlot));
        EventBus.Publish(new InventoryEvents.ItemAdded(this, fromItem, toSlot));
        return true;
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

    public bool IsEmpty()
    {
        return _items.Count == 0;
    }
}