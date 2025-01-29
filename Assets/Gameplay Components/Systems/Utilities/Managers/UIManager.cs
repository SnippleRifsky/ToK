using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<UIManager>();
                if (_instance == null)
                {
                    var go = new GameObject("UIManager");
                    _instance = go.AddComponent<UIManager>();
                }
            }

            return _instance;
        }
    }

    // UI References
    public CharacterPanel CharacterPanel { get; private set; }
    public TargetPanel TargetPanel { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeUIReferences();
    }

    private void InitializeUIReferences()
    {
        CharacterPanel = FindFirstObjectByType<CharacterPanel>();
        if (CharacterPanel is null)
        {
            Debug.LogError("UIManager: CharacterPanel not found!");
            return;
        }

        TargetPanel = FindFirstObjectByType<TargetPanel>();
        if (TargetPanel is null) Debug.LogError("UIManager: TargetPanel not found!");
    }
}