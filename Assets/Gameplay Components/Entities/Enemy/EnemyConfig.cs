using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "RPG Components/Enemy/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("Visual Settings")] public Mesh enemyMesh;
    public Material enemyMaterial;


    [Header("Entity Settings")] public string entityName = "Enemy";
    public int level = 1;
    public int xpValue;

    [Header("Base Stats")] public int attack = 10;
    public int defense = 10;
    public int maxHealth = 100;
    public int maxResource;
    public float healthRegen = 1f;
    public float resourceRegen;

    public BaseStats CreateBaseStats()
    {
        var stats = CreateInstance<BaseStats>();
        stats.attack = attack;
        stats.defense = defense;
        stats.maxHealth = maxHealth;
        stats.maxResource = maxResource;
        stats.healthRegen = healthRegen;
        stats.resourceRegen = resourceRegen;
        return stats;
    }
}