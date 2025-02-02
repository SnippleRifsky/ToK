using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance is not null) return _instance;
            _instance = FindFirstObjectByType<UIManager>();
            if (_instance is not null) return _instance;
            var go = new GameObject("UIManager");
            _instance = go.AddComponent<UIManager>();

            return _instance;
        }
    }

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

        UICanvas = FindFirstObjectByType<Canvas>();
        NameplateManager = gameObject.AddComponent<NameplateManager>();
        NameplateManager.Initialize();
    }
}