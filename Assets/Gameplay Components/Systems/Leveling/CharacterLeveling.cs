using TMPro;
using UnityEngine;


public class CharacterLeveling : MonoBehaviour
{
    // Temporary Leveling UI
    private TextMeshProUGUI _currentLevelText;
    private TextMeshProUGUI _currentXpText;
    private TextMeshProUGUI _xpToNextLevelText;
    private BaseXpSystem _xpSystemType;
    
    public int CharacterLevel { get; protected set; }
    
    private BaseXpSystem _baseXpSystem;

    private void Awake()
    {
        _baseXpSystem = ScriptableObject.CreateInstance<LinearXpSystem>();
        
        var textComponents = GetComponentsInChildren<TextMeshProUGUI>(); 
        foreach (var textComponent in textComponents)
        {
            switch (textComponent.name)
            {
                case "Current Level":
                    _currentLevelText = textComponent;
                    break;
                case "Current XP":
                    _currentXpText = textComponent;
                    break;
                case "XP To Level":
                    _xpToNextLevelText = textComponent;
                    break;
            }
        }
    }

    private void Start()
    {
        
        _baseXpSystem.GetLevelXpRange();
    }

    private void Update()
    {
        RefreshDisplays();
    }

    public void AddXp(int amount)
    {
        _baseXpSystem.AddXp(amount);
        CharacterLevel = _baseXpSystem.CurrentLevel;
    }

    private void RefreshDisplays()
    {
        _currentLevelText.text = $"Current Level: {_baseXpSystem.CurrentLevel}";
        _currentXpText.text = $"Current XP: {_baseXpSystem.CurrentXp}";
        _xpToNextLevelText.text = !_baseXpSystem.AtLevelCap ? $"XP To Next Level: {_baseXpSystem.XpToNextLevel()}" : $"XP To Next Level: At Max";
    }
}
