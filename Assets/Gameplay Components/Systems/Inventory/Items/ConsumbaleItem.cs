using UnityEngine;

public enum ConsumableType
{
    Health,
    Resource,
    Modifier
}

[CreateAssetMenu(fileName = "ConsumbaleItem", menuName = "RPG Components/Inventory System/Items/Consumable")]
public class ConsumbaleItem : ItemObject 
{
    public ConsumableType consumableType;
    public int value;
    public int duration;
    public StatModifier statModifier;
    public void Awake()
    {
        itemType = ItemType.Consumable;
    }
}