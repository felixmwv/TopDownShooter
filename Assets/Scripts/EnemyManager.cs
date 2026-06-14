using UnityEngine;

public class EnemyManager
{
    private const float SPAWN_RADIUS = 12f;
    private const float SPAWN_INTERVAL = 0.85f;

    private readonly Enemy[] _enemies;
    private float _spawnTimer;

    public EnemyManager(EnemyFactory factory, int poolSize)
    {
        _enemies = new Enemy[poolSize];
            
        for (int i = 0; i < poolSize; i++)
        {
            EnemyType type = i % 3 == 0 ? EnemyType.FAST : EnemyType.BASIC;
            _enemies[i] = factory.Create(type);
        }
    }

    public void Tick(float deltaTime, Vector2 playerPosition)
    {
        TickEnemies(deltaTime, playerPosition);
        TickSpawner(deltaTime, playerPosition);
    }
    
    public void DeactivateAll()
    {
        foreach (Enemy enemy in _enemies)
            if (enemy.IsActive) enemy.Deactivate();
    }

    public Enemy[] GetEnemies() => _enemies;

    private void TickEnemies(float deltaTime, Vector2 playerPosition)
    {
        foreach (Enemy enemy in _enemies)
            enemy.Tick(deltaTime, playerPosition);
    }

    private void TickSpawner(float deltaTime, Vector2 playerPosition)
    {
        _spawnTimer -= deltaTime;
        if (_spawnTimer > 0f) return;

        SpawnEnemy(playerPosition);
        _spawnTimer = SPAWN_INTERVAL;
    }

    private void SpawnEnemy(Vector2 playerPosition)
    {
        Enemy enemy = GetInactiveEnemy();
        if (enemy == null) return;

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector2 spawnPosition = playerPosition + new Vector2(
            Mathf.Cos(angle) * SPAWN_RADIUS,
            Mathf.Sin(angle) * SPAWN_RADIUS
        );

        enemy.Activate(spawnPosition);
    }

    private Enemy GetInactiveEnemy()
    {
        foreach (Enemy enemy in _enemies)
            if (!enemy.IsActive) return enemy;
        return null;
    }
}