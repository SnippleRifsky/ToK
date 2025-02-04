using UnityEngine;

public class CharacterLeveling : MonoBehaviour
{
    private LinearXpSystem _baseXpSystem;
    private Player _player;

    private void Awake()
    {
         _player = GameManager.Instance.Player;
        _baseXpSystem = ScriptableObject.CreateInstance<LinearXpSystem>();
    }

    private void Start()
    {
        _baseXpSystem.Initialize(_player);
    }

    public void AddXp(int amount)
    {
        if (_baseXpSystem != null)
        {
            _baseXpSystem.AddXp(amount);
        }
    }
}