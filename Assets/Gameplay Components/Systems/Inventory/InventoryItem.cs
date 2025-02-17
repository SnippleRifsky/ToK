using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "RPG Components/Systems/Loot/Item")]
public class InventoryItem : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private bool _isStackable;
    [SerializeField] private int _maxStackSize;
    [SerializeField] private int _quantity;

    public string Id
    {
        get => _id;
        private set => _id = value;
    }

    public string Name
    {
        get => _name;
        private set => _name = value;
    }

    public string Description
    {
        get => _description;
        private set => _description = value;
    }

    public Sprite Icon
    {
        get => _icon;
        private set => _icon = value;
    }

    public bool IsStackable
    {
        get => _isStackable;
        private set => _isStackable = value;
    }

    public int MaxStackSize
    {
        get => _maxStackSize;
        private set => _maxStackSize = value;
    }

    public int Quantity
    {
        get => _quantity;
        private set => _quantity = value;
    }
    
    public void Initialize(string id, string name, string description, Sprite icon, bool isStackable, int maxStackSize, int quantity = 1)
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
        var clone = ScriptableObject.CreateInstance<InventoryItem>();
        clone.Id = Id;
        clone.Name = Name;
        clone.Description = Description;
        clone.Icon = Icon;
        clone.IsStackable = IsStackable;
        clone.MaxStackSize = MaxStackSize;
        clone.SetQuantity(Quantity);
        return clone;
    }
}