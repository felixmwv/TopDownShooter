using UnityEngine;

public class Enemy
{
    public readonly Transform Transform;
    public readonly float ColliderRadius;
    public bool IsActive;

    private readonly float _speed;

    public Enemy(Transform transform, EnemyData data, float colliderRadius)
    {
        Transform = transform;
        ColliderRadius = colliderRadius;
        _speed = data.Speed;
        IsActive = false;
    }

    public void Tick(float deltaTime, Vector2 playerPosition)
    {
        if (!IsActive) return;

        Vector2 direction = (playerPosition - (Vector2)Transform.position).normalized;
        Transform.position += (Vector3)(direction * _speed * deltaTime);
    }

    public bool TakeDamage()
    {
        Deactivate();
        return true;
    }

    public void Activate(Vector2 position)
    {
        Transform.position = position;
        IsActive = true;
        Transform.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        IsActive = false;
        Transform.gameObject.SetActive(false);
    }
}