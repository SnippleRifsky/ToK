public class InventoryEvents
{
    public readonly struct CapacityChanged
    {
        public IInventorySystem Inventory { get; }
        public int NewCapacity { get; }
        public int OldCapacity { get; }

        public CapacityChanged(IInventorySystem inventory,int newCapacity, int oldCapacity)
        {
            Inventory = inventory;
            NewCapacity = newCapacity;
            OldCapacity = oldCapacity;
        }
    }
    
    public readonly struct ItemAdded
    {
        public IInventorySystem Inventory { get; }
        public InventoryItem Item { get; }
        public int SlotIndex { get; }
            
        public ItemAdded(IInventorySystem inventory, InventoryItem item, int slotIndex)
        {
            Inventory = inventory;
            Item = item;
            SlotIndex = slotIndex;
        }
    }

    public readonly struct ItemRemoved
    {
        public IInventorySystem Inventory { get; }
        public int SlotIndex { get; }
            
        public ItemRemoved(IInventorySystem inventory, int slotIndex)
        {
            Inventory = inventory;
            SlotIndex = slotIndex;
        }
    }

    public readonly struct ItemInteractionRequested
    {
        public InventoryItem Item { get; }
        public int SlotIndex { get; }
            
        public ItemInteractionRequested(InventoryItem item, int slotIndex)
        {
            Item = item;
            SlotIndex = slotIndex;
        }
    }

    public readonly struct MoveItem
    {
        public int FromSlot { get; }
        public int ToSlot { get; }

        public MoveItem(int fromSlot, int toSlot)
        {
            FromSlot = fromSlot;
            ToSlot = toSlot;
        }
    }
}