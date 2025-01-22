using UnityEngine;


[CreateAssetMenu(fileName = "BaseStats", menuName = "RPG Components/Stats/BaseStats")]

public class BaseStats : ScriptableObject
{
    public int attack = 10;
    public int defense = 10;
    public int maxHealth = 100;
    public int maxResource = 100;
    public float healthRegen = 1f;
    public float resourceRegen = 2f;
}