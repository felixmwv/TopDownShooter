using UnityEngine;

public class PlayerSystem
{
    private const float MOVE_FORCE = 15f;
    private const float BOOST_MULTIPLIER = 3f;
    private const float MAX_HP = 3f;

    private readonly Transform _transform;
    private readonly Transform _fireTransform;
    private readonly Rigidbody2D _rigidbody;
    private readonly Vector2 _halfBounds;

    private bool _isDead;
    public bool IsDead => _isDead;

    public PlayerSystem(Transform transform, Rigidbody2D rigidbody, Vector2 halfBounds, Transform fireTransform)
    {
        _transform = transform;
        _rigidbody = rigidbody;
        _halfBounds = halfBounds;
        _fireTransform = fireTransform;
    }

    public void Tick(float deltaTime, InputData input)
    {
        Rotate(input.MouseWorldPosition);
        Move(input);
        WrapPosition();
    }

    public void TakeDamage(float amount) => _isDead = true;

    public void Reset(Vector2 spawnPosition)
    {
        _isDead = false;
        _transform.position = spawnPosition;
        _rigidbody.linearVelocity = Vector2.zero;
    }

    private void Rotate(Vector2 mouseWorldPosition)
    {
        Vector2 dir = mouseWorldPosition - (Vector2)_transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        _transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Move(InputData input)
    {
        float force = input.BoostPressed ? MOVE_FORCE * BOOST_MULTIPLIER : MOVE_FORCE;
        _rigidbody.AddForce(input.MoveDirection * force);
        
        if (_fireTransform != null)
            _fireTransform.gameObject.SetActive(input.BoostPressed && input.MoveDirection != Vector2.zero);
    }

    private void WrapPosition()
    {
        Vector2 pos = _transform.position;

        if (pos.x > _halfBounds.x) pos.x = -_halfBounds.x;
        if (pos.x < -_halfBounds.x) pos.x = _halfBounds.x;
        if (pos.y > _halfBounds.y) pos.y = -_halfBounds.y;
        if (pos.y < -_halfBounds.y) pos.y = _halfBounds.y;

        _transform.position = pos;
    }
}