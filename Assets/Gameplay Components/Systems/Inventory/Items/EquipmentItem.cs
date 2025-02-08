using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Accessory,
    Default
}

[CreateAssetMenu(fileName = "EquipmentItem", menuName = "RPG Components/Inventory System/Items/Equipment")]
public class EquipmentItem : ItemObject
{
    
}