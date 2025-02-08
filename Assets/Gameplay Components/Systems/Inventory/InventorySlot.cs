[System.Serializable]
public class InventorySlot
{
    public ItemObject Item;
    public int Amount;

    public InventorySlot(ItemObject item, int amount)
    {
        Item = item;
        Amount = amount;
    }

    public void AddToStack(int amount)
    {
        Amount += amount;
    }
}