using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootablePanel : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private int initialCapacity = 10;
    [SerializeField] private int maxCapacity = 100;

    private List<InventorySlot> _slots;
    private RectTransform _rectTransform;
    private IInventorySystem _lootInventory;

    private bool _isInitialized = false;

    private void Awake()
    {
        // Check if RectTransform exists
        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null)
        {
            _rectTransform = gameObject.AddComponent<RectTransform>();
        }

        // Check if GridLayoutGroup exists
        if (gridLayoutGroup == null)
        {
            gridLayoutGroup = GetComponent<GridLayoutGroup>();
            if (gridLayoutGroup == null)
            {
                gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
            }
        }

        // Initialize components
        _slots = new List<InventorySlot>();
        
        EventBus.Subscribe<LootEvents.LootRequested>(OnLootRequested);
        EventBus.Subscribe<InventoryEvents.ItemAdded>(OnItemAdded);
        EventBus.Subscribe<InventoryEvents.ItemRemoved>(OnItemRemoved);
        
        _isInitialized = true;
        UpdateLayout();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnLootRequested(LootEvents.LootRequested evt)
    {
        // Exit if the source is not lootable
        if (evt.Source is not ILootable lootable) return;
        
        // Assign the clicked lootable's inventory to the panel
        _lootInventory = lootable.GetInventory();

        // Initialize the slots based on the lootable's inventory capacity
        InitializeSlots(_lootInventory.Capacity);
        
        _rectTransform.position = Input.mousePosition;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized)
        {
            UpdateLayout();
        }
    }

    private void InitializeSlots(int count)
    {
        // Clear existing slots if any
        foreach (var slot in _slots)
        {
            if (slot is not null && slot.gameObject is not null)
                Destroy(slot.gameObject);
        }
        _slots.Clear();

        // Create new slots and set items
        for (int i = 0; i < count; i++)
        {
            CreateSlot(i);
            var item = _lootInventory.GetItem(i);
            if (item is not null)
            {
                _slots[i].SetItem(item);
            }
        }
    }

    private void CreateSlot(int index)
    {
        var slotObject = Instantiate(slotPrefab, gridLayoutGroup.transform);
        var slot = slotObject.GetComponent<InventorySlot>();

        slot.Initialize(_lootInventory, index);
        _slots.Add(slot);
    }

    private void UpdateLayout()
    {
        if (!_isInitialized) return;
        
        // Calculate the optimal grid size based on the panel width
        float availableWidth = _rectTransform.rect.width - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right);
        float slotWidth = gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x;
        
        int slotsPerRow = Mathf.Max(1, Mathf.FloorToInt(availableWidth / slotWidth));
        
        // Update the grid layout
        gridLayoutGroup.constraintCount = slotsPerRow;
    }

    private void OnCapacityChanged(InventoryEvents.CapacityChanged evt)
    {
        InitializeSlots(evt.NewCapacity);
        UpdateLayout();
    }

    private void OnItemAdded(InventoryEvents.ItemAdded evt)
    {
        if (evt.Inventory != _lootInventory) return;
        if (evt.SlotIndex >= 0 && evt.SlotIndex < _slots.Count)
        {
            _slots[evt.SlotIndex].SetItem(evt.Item);
        }
    }

    private void OnItemRemoved(InventoryEvents.ItemRemoved evt)
    {
        if (evt.Inventory != _lootInventory) return;
        if (evt.SlotIndex >= 0 && evt.SlotIndex < _slots.Count)
        {
            _slots[evt.SlotIndex].Clear();
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        if (_isInitialized && gameObject.activeInHierarchy)
        {
            UpdateLayout();
        }
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<LootEvents.LootRequested>(OnLootRequested);
        EventBus.Unsubscribe<InventoryEvents.ItemAdded>(OnItemAdded);
        EventBus.Unsubscribe<InventoryEvents.ItemRemoved>(OnItemRemoved);
    }
}