using UnityEngine;

public class ShootingSystem
{
    private const float FIRE_RATE = 0.1f;

    private readonly BulletPool _bulletPool;
    private readonly Transform _playerTransform;
    private float _cooldown;

    public ShootingSystem(BulletPool bulletPool, Transform playerTransform)
    {
        _bulletPool = bulletPool;
        _playerTransform = playerTransform;
    }

    public void Tick(float deltaTime, InputData input)
    {
        _cooldown -= deltaTime;

        if (!input.ShootPressed || _cooldown > 0f) return;

        Vector2 direction = (input.MouseWorldPosition - (Vector2)_playerTransform.position).normalized;
        _bulletPool.Fire(_playerTransform.position, direction);
        _cooldown = FIRE_RATE;
    }
}