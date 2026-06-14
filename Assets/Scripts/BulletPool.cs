using UnityEngine;

public class BulletPool
{
    private const float BULLET_SPEED = 18f;
    private const float MAX_RANGE = 30f;

    private readonly Bullet[] _bullets;
    private readonly Transform _playerTransform;
    public Bullet[] GetBullets() => _bullets;
    
    public BulletPool(Transform[] bulletTransforms, Transform playerTransform)
    {
        _playerTransform = playerTransform;
        _bullets = new Bullet[bulletTransforms.Length];

        for (int i = 0; i < bulletTransforms.Length; i++)
        {
            var sr = bulletTransforms[i].GetComponent<SpriteRenderer>();
            float radius = sr != null
                ? Mathf.Min(sr.bounds.extents.x, sr.bounds.extents.y) * 4f
                : 0.1f;

            _bullets[i] = new Bullet(bulletTransforms[i], radius);
            bulletTransforms[i].gameObject.SetActive(false);
        }
    }

    public void Tick(float deltaTime)
    {
        foreach (Bullet bullet in _bullets)
        {
            if (!bullet.IsActive) continue;

            bullet.Transform.position += (Vector3)(bullet.Velocity * deltaTime);

            if (Vector2.Distance(bullet.Transform.position, _playerTransform.position) > MAX_RANGE)
                Deactivate(bullet);
        }
    }

    public void Fire(Vector2 position, Vector2 direction)
    {
        Bullet bullet = GetInactiveBullet();
        if (bullet == null) return;

        bullet.Transform.position = position;
        bullet.Velocity = direction.normalized * BULLET_SPEED;
        bullet.IsActive = true;
        bullet.Transform.gameObject.SetActive(true);
    }

    public void Deactivate(Bullet bullet)
    {
        bullet.IsActive = false;
        bullet.Transform.gameObject.SetActive(false);
    }

    private Bullet GetInactiveBullet()
    {
        foreach (Bullet bullet in _bullets)
            if (!bullet.IsActive) return bullet;
        return null;
    }
}
