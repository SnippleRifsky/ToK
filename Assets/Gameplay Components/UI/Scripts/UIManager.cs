using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CharacterPanel characterPanel;
    private PlayerData _playerData;
    
    private void Awake()
    {
        Debug.Log("[UIManager] Awake - CharacterPanel reference state: " + 
                  (characterPanel != null ? "Set" : "Null"));
    }

    private void OnEnable()
    {
        Debug.Log("[UIManager] OnEnable - CharacterPanel reference state: " + 
                  (characterPanel != null ? "Set" : "Null"));
    }
    private void Start()
    {
        Debug.Log("[UIManager] Start - CharacterPanel reference state: " + 
                  (characterPanel != null ? "Set" : "Null"));
        
        // If it's null, let's try to find it in the scene
        if (characterPanel == null)
        {
            characterPanel = Object.FindFirstObjectByType<CharacterPanel>();
            Debug.Log("[UIManager] Attempted to find CharacterPanel in scene: " + 
                      (characterPanel != null ? "Found" : "Not Found"));
        }

        InitializeAllPanels();
    }
    
    private void InitializeAllPanels()
    {
        Debug.Log("[UIManager] InitializeAllPanels - CharacterPanel reference state: " + 
                  (characterPanel != null ? "Set" : "Null"));
        InitializeCharacterPanel();
    }
    
    private void InitializeCharacterPanel()
    {
        Debug.Log("[UIManager] InitializeCharacterPanel called");
        if (characterPanel == null)
        {
            Debug.LogError("[UIManager] CharacterPanel is null. Hierarchy path should be: Canvas -> Character Panel");
            
            // Let's try to find it and print all CharacterPanels in the scene for debugging
            var allPanels = Object.FindObjectsByType<CharacterPanel>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            if (allPanels.Length > 0)
            {
                Debug.Log("[UIManager] Found " + allPanels.Length + " CharacterPanel(s) in scene:");
                foreach (var panel in allPanels)
                {
                    Debug.Log($"[UIManager] Panel found at path: {GetGameObjectPath(panel.gameObject)}");
                }
            }
            else
            {
                Debug.Log("[UIManager] No CharacterPanels found in scene");
            }
            return;
        }
        Debug.Log("[UIManager] CharacterPanel found. Initializing...");
        characterPanel = GetComponentInChildren<CharacterPanel>();
        _playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
        if (characterPanel == null || _playerData == null) return;
        characterPanel.Initalize(_playerData);
    }
    
    // Utility method to print the full path of a GameObject
    private string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform parent = obj.transform.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        return path;
    }
}