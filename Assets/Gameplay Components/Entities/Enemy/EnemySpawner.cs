using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private SpawnerConfig spawnerConfig;

    [Header("Debug Settings"), SerializeField] 
    private bool showSpawnArea = true;

    [SerializeField] private Color spawnAreaColor = new(1f, 0f, 0f, 0.2f);

    private readonly List<Enemy> _spawnedEnemies = new();
    private float _nextSpawnTime;

    public bool CanSpawn => spawnerConfig is not null && _spawnedEnemies.Count < spawnerConfig.MaxEnemies;

    private void Awake()
    {
        if (spawnerConfig is null)
        {
            Debug.LogError("No SpawnerConfig assigned!", this);
            enabled = false;
            return;
        }

        EventBus.Subscribe<EntityEvents.EntityDeathEvent>(OnEnemyDeath);
    }

    private void Update()
    {
        if (!spawnerConfig.AutoSpawn) return;

        if (Time.time >= _nextSpawnTime)
        {
            if (CanSpawn)
                switch (spawnerConfig.UseWaves)
                {
                    case true:
                        SpawnToMax();
                        break;
                    case false:
                        TrySpawnEnemy();
                        break;
                }

            _nextSpawnTime = Time.time + spawnerConfig.SpawnInterval;
        }
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<EntityEvents.EntityDeathEvent>(OnEnemyDeath);
    }

    private void OnEnemyDeath(EntityEvents.EntityDeathEvent evt)
    {
        if (evt.Entity is Enemy enemy) _spawnedEnemies.Remove(enemy);
    }

    public bool TrySpawnEnemy()
    {
        if (!CanSpawn) return false;

        var randomPosition = GetRandomSpawnPosition();
        var enemy = SpawnEnemy(randomPosition);

        if (enemy is not null)
        {
            _spawnedEnemies.Add(enemy);
            return true;
        }

        return false;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        var randomPoint = Random.insideUnitCircle * spawnerConfig.SpawnRadius;
        var spawnPosition = transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);

        return spawnPosition;
    }

    private Enemy SpawnEnemy(Vector3 position)
    {
        var prefab = spawnerConfig.GetRandomEnemyPrefab();
        if (prefab is null) return null;

        // Instantiate the prefab at the spawn position
        var enemyObject = Instantiate(prefab, position, Quaternion.identity);
        var enemy = enemyObject.GetComponent<Enemy>();

        if (enemy is null)
        {
            Debug.LogError("Spawned prefab does not have an Enemy component!", enemyObject);
            Destroy(enemyObject);
            return null;
        }

        return enemy;
    }

    public void SpawnSingleEnemy()
    {
        TrySpawnEnemy();
    }

    public void SpawnToMax()
    {
        while (CanSpawn) TrySpawnEnemy();
    }

    private void OnDrawGizmos()
    {
        if (!showSpawnArea || spawnerConfig is null) return;

        Gizmos.color = spawnAreaColor;
        Gizmos.DrawSphere(transform.position, spawnerConfig.SpawnRadius);
    }
}