using UnityEngine;

public class Bullet
{
    public readonly Transform Transform;
    public bool IsActive;
    public Vector2 Velocity;

    public Bullet(Transform transform)
    {
        Transform = transform;
        IsActive = false;
    }
}
