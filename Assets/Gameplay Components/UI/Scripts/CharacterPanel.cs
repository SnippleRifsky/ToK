using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    private Slider _healthBar;
    private IResourceProvider _resourceProvider;
    private Slider _resourceBar;

    private void Awake()
    {
        _healthBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Health Bar");
        _resourceBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Resource Bar");

        _resourceProvider = GameObject.FindGameObjectWithTag("Player").GetComponent<IResourceProvider>();
        if (_resourceProvider is not null)
        {
            _resourceProvider.OnHealthChanged += UpdateHealthBar;
            _resourceProvider.OnResourceChanged += UpdateResourceBar;
        }
        else
        {
            Debug.LogWarning("No player data found");
        }
    }

    private void OnDestroy()
    {
        _resourceProvider.OnHealthChanged -= UpdateHealthBar;
        _resourceProvider.OnResourceChanged -= UpdateResourceBar;
    }

    private void UpdateHealthBar(float newHealth)
    {
        _healthBar.value = newHealth / _resourceProvider.MaxHealth;
    }

    private void UpdateResourceBar(float newResource)
    {
        _resourceBar.value = newResource / _resourceProvider.MaxResource;
    }
}