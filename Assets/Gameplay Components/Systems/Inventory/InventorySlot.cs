using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
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
        if (eventData.button != PointerEventData.InputButton.Right || IsEmpty) return;
        EventBus.Publish(new InventoryEvents.ItemInteractionRequested(Item, SlotIndex));
        Debug.Log("Use Item: " + Item.Name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        eventData.pointerDrag = gameObject;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        int fromSlot = SlotIndex;
        int toSlot = -1;
        
        PointerEventData pointerData = new PointerEventData (EventSystem.current)
        {
            pointerId = -1,
        };
		
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var obj in results.Where(obj => obj.gameObject.GetComponent<InventorySlot>() != null))
        {
            toSlot = obj.gameObject.GetComponent<InventorySlot>().SlotIndex;
        }
        
        if (fromSlot == toSlot || toSlot == -1) return;
        GameManager.Instance.InventorySystem.MoveItem(fromSlot, toSlot);
    }
}