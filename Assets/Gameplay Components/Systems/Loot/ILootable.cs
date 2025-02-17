public interface ILootable
{
    void OnLootRequested(LootEvents.LootRequested evt);
    IInventorySystem GetInventory();
}