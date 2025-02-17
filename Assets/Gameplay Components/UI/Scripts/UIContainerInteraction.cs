using UnityEngine;

public class UIContainerInteraction : MonoBehaviour
{
    private Canvas _canvas;
    private CursorRaycastService _cursorRaycastService;
    
    public void Start()
    {
        _canvas = UIManager.Instance.UICanvas;
        _cursorRaycastService = GameManager.Instance.CursorRaycastService;
    }
    
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var lootable = _cursorRaycastService.TryGetLootableUnderCursor();
            if ( lootable != null)
            {
                EventBus.Publish(new LootEvents.LootRequested(lootable, GameManager.Instance.PlayerInventory));
            }
        }
    }
}