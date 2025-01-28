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
                _instance = Object.FindFirstObjectByType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }
    
    public bool IsInitialized { get; set; }
    
    public Player Player { get; private set; }
    
    public Camera PlayerCamera { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeReferences();
        IsInitialized = true;
    }

    private void InitializeReferences()
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
    }
}