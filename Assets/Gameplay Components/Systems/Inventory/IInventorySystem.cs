public interface IInventorySystem
{
    int Capacity { get; set; }
    bool AddItem(InventoryItem item, int slotIndex = -1);
    bool RemoveItem(int slotIndex, int quantity = 1);
    InventoryItem GetItem(int slotIndex);

    bool IsEmpty();
    void Clear();
}