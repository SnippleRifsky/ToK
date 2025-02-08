using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private int initalCapacity = 10;
    [SerializeField] private int maxCapacity = 100;

    private List<InventorySlot> _slots;
    private RectTransform _rectTransform;
    private InventorySystem _inventorySystem;

    private void Awake()
    {
        _slots = new List<InventorySlot>();
        _rectTransform = GetComponent<RectTransform>();
        _inventorySystem = new InventorySystem(initalCapacity);

        InitializeSlots(initalCapacity);
        UpdateLayout();
        
        EventBus.Subscribe<InventoryEvents.CapacityChanged>(OnCapacityChanged);
        EventBus.Subscribe<InventoryEvents.ItemAdded>(OnItemAdded);
        EventBus.Subscribe<InventoryEvents.ItemRemoved>(OnItemRemoved);
    }

    private void InitializeSlots(int count)
    {
        // Clear exisiting slots if any
        foreach (var slot in _slots)
        {
            Destroy(slot.gameObject);
        }
        _slots.Clear();
        
        // Create new slots
        for (int i = 0; i < count; i++)
        {
            CreateSlot(i);
        }
    }

    private void CreateSlot(int index)
    {
        var slotObject = Instantiate(slotPrefab, gridLayoutGroup.transform);
        var slot = slotObject.GetComponent<InventorySlot>();
        slot.Initialize(index);
        _slots.Add(slot);
    }

    private void UpdateLayout()
    {
        // Calculate the optimal grid size based on the panel width
        float availableWidth = _rectTransform.rect.width;
        float slotWidth = availableWidth / gridLayoutGroup.spacing.x;
        
        int slotsPerRow = Mathf.Max(1, Mathf.FloorToInt(availableWidth / slotWidth));
        
        // Update the grid layout
        gridLayoutGroup.constraintCount = slotsPerRow;
    }
    
    private void OnCapacityChanged(InventoryEvents.CapacityChanged evt)
    {
        if (evt.NewCapacity > maxCapacity)
        {
            Debug.LogWarning($"Attempted to exceed max inventory capacity of {maxCapacity}");
            return;
        }

        // Add or remove slots as needed
        int difference = evt.NewCapacity - _slots.Count;
        
        if (difference > 0)
        {
            // Add new slots
            for (int i = 0; i < difference; i++)
            {
                CreateSlot(_slots.Count);
            }
        }
        else if (difference < 0)
        {
            // Remove slots from the end
            for (int i = 0; i < -difference; i++)
            {
                var slot = _slots[_slots.Count - 1];
                _slots.RemoveAt(_slots.Count - 1);
                Destroy(slot.gameObject);
            }
        }

        UpdateLayout();
    }
    
    private void OnItemAdded(InventoryEvents.ItemAdded evt)
    {
        if (evt.SlotIndex >= 0 && evt.SlotIndex < _slots.Count)
        {
            _slots[evt.SlotIndex].SetItem(evt.Item);
        }
    }
    
    private void OnItemRemoved(InventoryEvents.ItemRemoved evt)
    {
        if (evt.SlotIndex >= 0 && evt.SlotIndex < _slots.Count)
        {
            _slots[evt.SlotIndex].Clear();
        }
    }
    
    private void OnRectTransformDimensionsChange()
    {
        UpdateLayout();
    }
    
    private void OnDestroy()
    {
        EventBus.Unsubscribe<InventoryEvents.CapacityChanged>(OnCapacityChanged);
        EventBus.Unsubscribe<InventoryEvents.ItemAdded>(OnItemAdded);
        EventBus.Unsubscribe<InventoryEvents.ItemRemoved>(OnItemRemoved);
    }
}