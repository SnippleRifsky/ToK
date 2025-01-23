using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEntityNameplate : MonoBehaviour
{
    private Entity _entity;
    private Camera _playerCamera;
    private Canvas _canvas;
    private Slider _healthBar;
    private TextMeshProUGUI _levelLabel;
    private TextMeshProUGUI _nameLabel;


    private void Awake()
    {
        _entity = gameObject.GetComponent<Entity>();
        _levelLabel = GetComponentsInChildren<TextMeshProUGUI>()
            .FirstOrDefault(text => text.gameObject.name == "Level Label");
        _nameLabel = GetComponentsInChildren<TextMeshProUGUI>()
            .FirstOrDefault(text => text.gameObject.name == "Entity Name");
    }

    private void Start()
    {
        _playerCamera = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>();
        _canvas = GetComponentInChildren<Canvas>();

        _healthBar = GetComponentsInChildren<Slider>()
            .FirstOrDefault(slider => slider.gameObject.name == "Health Bar");
        if (_entity.Stats is not null)
            _entity.Stats.Resources.OnHealthChanged += UpdateHealthBar;
        else
            Debug.LogWarning("Failed to bind to OnHealthChanged event");
    }

    private void Update()
    {
        UpdateUI();
    }

    private void OnDestroy()
    {
        _entity.Stats.Resources.OnHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float newHealth)
    {
        _healthBar.value = newHealth / _entity.Stats.MaxHealth;
    }

    private void RotateUI()
    {
        _canvas.transform.LookAt(_playerCamera.transform);
    }

    private void UpdateUIText()
    {
        _levelLabel.text = _entity.Level.ToString();
        _nameLabel.text = _entity.EntityName;
    }

    private void UpdateUI()
    {
        RotateUI();
        UpdateUIText();
    }
}