using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                AssignSingletonInstance();
            }
            return _instance;
        }
    }

    public bool IsInitialized { get; private set; }
    public UIManager UIManager { get; private set; }
    public Player Player { get; private set; }
    public Camera PlayerCamera { get; private set; }
    
    public InventorySystem InventorySystem { get; private set; }

    private void Awake()
    {
        if (IsDuplicateInstance())
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeGameDependencies();
        IsInitialized = true;
    }

    private static void AssignSingletonInstance()
    {
        _instance = FindFirstObjectByType<GameManager>();
        if (_instance is not null) return;
        var gameManagerObject = new GameObject("GameManager");
        _instance = gameManagerObject.AddComponent<GameManager>();
    }

    private bool IsDuplicateInstance() => _instance != null && _instance != this;

    private void InitializeGameDependencies()
    {
        Player = FindFirstObjectByType<Player>();
        if (Player is null)
        {
            Debug.LogError("GameManager: Player not found!");
            return;
        }

        PlayerCamera = Player.GetComponentInChildren<Camera>();
        if (PlayerCamera is null)
        {
            Debug.LogError("GameManager: Player Camera not found!");
        }
        
        // statically assign initial capacity (pull from SavedData later)
        InventorySystem = new InventorySystem(10); 
        
        UIManager = UIManager.Instance;
    }
}