using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SpawnerConfig", menuName = "RPG Components/Spawner/SpawnerConfig")]
public class SpawnerConfig : ScriptableObject
{
    [Serializable]
    public class SpawnableEnemy
    {
        public GameObject enemyPrefab;
        [Range(0f, 1f)] public float spawnWeight = 1f;
    }

    [Header("Spawn Settings")] [SerializeField]
    private List<SpawnableEnemy> enemyTypes = new();

    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private bool autoSpawn;
    
    [Header("Wave Settings")]
    [SerializeField] private bool useWaves;
    [SerializeField] private int enemiesPerWave = 5;

    public int MaxEnemies => maxEnemies;
    public float SpawnRadius => spawnRadius;
    public bool AutoSpawn => autoSpawn;
    public float SpawnInterval => spawnInterval;
    public bool UseWaves => useWaves;
    public int EnemiesPerWave => enemiesPerWave;

    public GameObject GetRandomEnemyPrefab()
    {
        if (enemyTypes == null || enemyTypes.Count == 0)
        {
            Debug.LogError($"No enemy types configured in {name}");
            return null;
        }

        var totalWeight = 0f;
        foreach (var enemy in enemyTypes) totalWeight += enemy.spawnWeight;

        var random = Random.Range(0f, totalWeight);
        var currentWeight = 0f;

        foreach (var enemy in enemyTypes)
        {
            currentWeight += enemy.spawnWeight;
            if (random <= currentWeight) return enemy.enemyPrefab;
        }

        return enemyTypes[0].enemyPrefab;
    }
}