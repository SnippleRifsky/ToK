using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "RPG Components/Inventory/Item", order = 0)]
public class Item : ScriptableObject
{
    public int itemId;
    public string itemName;
    public int itemValue;
    public Sprite itemIcon;
}