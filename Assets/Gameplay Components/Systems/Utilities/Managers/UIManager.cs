using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    private const string UIManagerGameObjectName = "UIManager";

    public static UIManager Instance => _instance ??= GetOrCreateInstance();

    public CharacterPanel CharacterPanel { get; private set; }
    public TargetPanel TargetPanel { get; private set; }
    public Canvas UICanvas { get; private set; }
    public NameplateManager NameplateManager { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeUIComponents();
    }

    private static UIManager GetOrCreateInstance()
    {
        var existingInstance = FindFirstObjectByType<UIManager>();
        if (existingInstance != null) return existingInstance;

        var go = new GameObject(UIManagerGameObjectName);
        return go.AddComponent<UIManager>();
    }

    private void InitializeUIComponents()
    {
        CharacterPanel = TryFindComponent<CharacterPanel>();
        TargetPanel = TryFindComponent<TargetPanel>();
        UICanvas = TryFindComponent<Canvas>();

        NameplateManager = gameObject.AddComponent<NameplateManager>();
        NameplateManager.Initialize();
    }

    private T TryFindComponent<T>() where T : Component
    {
        var component = FindFirstObjectByType<T>();
        if (component == null)
        {
            Debug.LogError($"{nameof(UIManager)}: {typeof(T).Name} not found!");
        }
        return component;
    }
}