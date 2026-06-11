using UnityEngine;

public class PlayerSystem
{
    private const float MOVE_FORCE = 15f;

    private readonly Transform _transform;
    private readonly Rigidbody2D _rigidbody;

    public PlayerSystem(Transform transform, Rigidbody2D rigidbody)
    {
        _transform = transform;
        _rigidbody = rigidbody;
    }

    public void Tick(float deltaTime, InputData input)
    {
        Rotate(input.MouseWorldPosition);
        Move(input);
    }

    private void Rotate(Vector2 mouseWorldPosition)
    {
        Vector2 dir = mouseWorldPosition - (Vector2)_transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        _transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Move(InputData input)
    {
        Vector2 forward = (input.MouseWorldPosition - (Vector2)_transform.position).normalized;
        Vector2 right = new Vector2(forward.y, -forward.x);

        Vector2 moveDir = forward * input.MoveDirection.y + right * input.MoveDirection.x;
        _rigidbody.AddForce(moveDir * MOVE_FORCE);
    }
}