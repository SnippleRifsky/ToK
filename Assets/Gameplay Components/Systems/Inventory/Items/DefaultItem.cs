using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultItem", menuName = "RPG Components/Inventory System/Items/Default")]
public class DefaultItem : ItemObject
{
    public void Awake()
    {
        itemType = ItemType.Default;
    }
}