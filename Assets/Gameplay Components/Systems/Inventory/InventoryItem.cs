using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Sprite Icon { get; private set; }
    public bool IsStackable { get; private set; }
    public int MaxStackSize { get; private set; }
    public int Quantity { get; private set; }

    public InventoryItem(string id, string name, string description, Sprite icon, bool isStackable, int maxStackSize, int quantity = 1)
    {
        Id = id;
        Name = name;
        Description = description;
        Icon = icon;
        IsStackable = isStackable;
        MaxStackSize = maxStackSize;
        Quantity = quantity;
    }

    public bool CanStackWith(InventoryItem other)
    {
        return IsStackable && other.IsStackable && other.Id == Id && Quantity < MaxStackSize;
    }

    public void SetQuantity(int quantity)
    {
        Quantity = Mathf.Clamp(quantity, 0, IsStackable ? MaxStackSize : 1);
    }

    public InventoryItem Clone()
    {
        var clone = new InventoryItem(Id, Name, Description, Icon, IsStackable, MaxStackSize);
        clone.SetQuantity(Quantity);
        return clone;
    }
}