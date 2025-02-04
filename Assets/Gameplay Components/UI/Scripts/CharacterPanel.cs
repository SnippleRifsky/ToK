using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    private Slider _healthBar;
    private Player _player;
    private Slider _resourceBar;

    private TextMeshProUGUI _currentLevelText;
    private TextMeshProUGUI _currentXpText;
    private BaseXpSystem _xpSystemType;
    private TextMeshProUGUI _xpToNextLevelText;

    private void Awake()
    {
        _healthBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Health Bar");
        _resourceBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Resource Bar");

        var textComponents = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var textComponent in textComponents)
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

        _player = GameManager.Instance.Player;
        EventBus.Subscribe<EntityEvents.HealthChanged>(OnHealthChanged);
        EventBus.Subscribe<EntityEvents.ResourceChanged>(OnResourceChanged);
        EventBus.Subscribe<PlayerEvents.LevelingChanged>(OnLevelingChanged);
    }


    private void OnDestroy()
    {
        EventBus.Unsubscribe<EntityEvents.HealthChanged>(OnHealthChanged);
        EventBus.Unsubscribe<EntityEvents.ResourceChanged>(OnResourceChanged);
        EventBus.Unsubscribe<PlayerEvents.LevelingChanged>(OnLevelingChanged);
    }

    private void OnHealthChanged(EntityEvents.HealthChanged evt)
    {
        if (!ReferenceEquals(evt.Entity, _player)) return;
        _healthBar.value = evt.CurrentHealth / evt.MaxHealth;
    }

    private void OnResourceChanged(EntityEvents.ResourceChanged evt)
    {
        if ((Player)evt.Entity != _player) return;
        _resourceBar.value = evt.CurrentResource / evt.MaxResource;
    }

    private void OnLevelingChanged(PlayerEvents.LevelingChanged evt)
    {

        if (!ReferenceEquals(evt.Player, _player)) return;

        _currentLevelText.text = $"Current Level: {evt.CurrentLevel}";
        _currentXpText.text = $"Current XP: {evt.CurrentXp}";
        _xpToNextLevelText.text = !evt.AtLevelCap 
            ? $"XP To Next Level: {evt.XpToNextLevel}" 
            : "XP To Next Level: At Max";
    }
}