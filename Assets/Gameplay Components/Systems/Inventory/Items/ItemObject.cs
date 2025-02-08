using UnityEngine;

public enum ItemType
{
	Consumable,
	Equipment,
	Default
}

public abstract class ItemObject : ScriptableObject
{
	public GameObject itemPrefab;
	public ItemType itemType;
	[TextArea(15,20)]
	public string itemDescription;
}