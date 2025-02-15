public interface IInventorySystem
{
    int Capacity { get; set; }
    bool AddItem(InventoryItem item, int slotIndex = -1);
    bool RemoveItem(int slotIndex, int quantity = 1);
    bool MoveItem(int fromSlot, int toSlot);
    InventoryItem GetItem(int slotIndex);
    bool HasItem(string itemId, out int slotIndex);
    int GetItemCount(string itemId);
    void Clear();
}