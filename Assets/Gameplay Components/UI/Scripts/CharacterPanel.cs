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

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (_player is not null)
        {
            _player.Stats.Resources.OnHealthChanged += UpdateHealthBar;
            _player.Stats.Resources.OnResourceChanged += UpdateResourceBar;
        }
        else
        {
            Debug.LogWarning("No player data found");
        }
    }

    private void OnDestroy()
    {
        _player.Stats.Resources.OnHealthChanged -= UpdateHealthBar;
        _player.Stats.Resources.OnResourceChanged -= UpdateResourceBar;
    }

    private void UpdateHealthBar(float newHealth)
    {
        _healthBar.value = newHealth / _player.Stats.MaxHealth;
    }

    private void UpdateResourceBar(float newResource)
    {
        _resourceBar.value = newResource / _player.Stats.MaxResource;
    }
}