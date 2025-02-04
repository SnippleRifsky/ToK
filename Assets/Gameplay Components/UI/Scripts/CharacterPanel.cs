using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    private Slider _healthBar;
    private Player _player;
    private Slider _resourceBar;

    private void Awake()
    {
        _healthBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Health Bar");
        _resourceBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Resource Bar");

        _player = GameManager.Instance.Player;
        EventBus.Subscribe<EntityEvents.HealthChanged>(OnHealthChanged);
        EventBus.Subscribe<EntityEvents.ResourceChanged>(OnResourceChanged);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<EntityEvents.HealthChanged>(OnHealthChanged);
        EventBus.Unsubscribe<EntityEvents.ResourceChanged>(OnResourceChanged);
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
}