using TMPro;
using UnityEngine;


public class CharacterLeveling : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI currentXpText;
    [SerializeField] private TextMeshProUGUI xpToNextLevelText;
    [SerializeField] private BaseXpSystem xpSystemType;
    
    public int CharacterLevel { get; private set; }
    
    private BaseXpSystem _baseXpSystem;

    private void Awake()
    {
        _baseXpSystem = ScriptableObject.Instantiate(xpSystemType);
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
        currentLevelText.text = $"Current Level: {_baseXpSystem.CurrentLevel}";
        currentXpText.text = $"Current XP: {_baseXpSystem.CurrentXp}";
        xpToNextLevelText.text = !_baseXpSystem.AtLevelCap ? $"XP To Next Level: {_baseXpSystem.XpToNextLevel()}" : $"XP To Next Level: At Max";
    }
}
