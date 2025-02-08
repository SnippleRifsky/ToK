public class InventoryEvents
{
    public readonly struct CapacityChanged
    {
        public int NewCapacity { get; }
        public int OldCapacity { get; }

        public CapacityChanged(int newCapacity, int oldCapacity)
        {
            NewCapacity = newCapacity;
            OldCapacity = oldCapacity;
        }
    }
    
    public readonly struct ItemAdded
    {
        public InventoryItem Item { get; }
        public int SlotIndex { get; }
            
        public ItemAdded(InventoryItem item, int slotIndex)
        {
            Item = item;
            SlotIndex = slotIndex;
        }
    }

    public readonly struct ItemRemoved
    {
        public int SlotIndex { get; }
            
        public ItemRemoved(int slotIndex)
        {
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
}