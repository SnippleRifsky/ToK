using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Image backgroundImage;

    public bool IsEmpty => Item == null;
    public InventoryItem Item { get; private set; }

    public int SlotIndex { get; private set; }

    public void Initialize(int index)
    {
        SlotIndex = index;
        Clear();
    }

    public void SetItem(InventoryItem item)
    {
        Item = item;
        UpdateVisuals();
    }

    public void Clear()
    {
        Item = null;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (Item == null)
        {
            itemIcon.enabled = false;
            quantityText.enabled = false;
            return;
        }

        itemIcon.enabled = true;
        itemIcon.sprite = Item.Icon;

        quantityText.enabled = Item.IsStackable;
        if (Item.IsStackable) quantityText.text = Item.Quantity.ToString();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            EventBus.Publish(new InventoryEvents.ItemInteractionRequested(Item, SlotIndex));
    }
}