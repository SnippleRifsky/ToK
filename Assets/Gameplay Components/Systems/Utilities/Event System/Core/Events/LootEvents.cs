public class LootEvents
{
    public readonly struct LootRequested
    {
        // Container that is being looted
        public ILootable Source { get; }
        
        // The IInventorySystem that will receive the loot
        public IInventorySystem Inventory { get; }

        public LootRequested(ILootable source,  IInventorySystem inventory)
        {
            Source = source;
            Inventory = inventory;
        }
    }
}