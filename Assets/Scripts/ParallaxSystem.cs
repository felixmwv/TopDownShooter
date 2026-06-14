using UnityEngine;

public class ParallaxSystem
{
    private const float PARALLAX_STRENGTH = 0.05f;
    private const float SMOOTHING = 5f;

    private readonly Transform _backgroundTransform;
    private readonly Vector3 _originPosition;

    public ParallaxSystem(Transform backgroundTransform)
    {
        _backgroundTransform = backgroundTransform;
        _originPosition = backgroundTransform.position;
    }

    public void Tick(float deltaTime, Vector2 playerPosition)
    {
        Vector3 targetPosition = _originPosition + new Vector3(
            -playerPosition.x * PARALLAX_STRENGTH,
            -playerPosition.y * PARALLAX_STRENGTH,
            0f
        );

        _backgroundTransform.position = Vector3.Lerp(
            _backgroundTransform.position,
            targetPosition,
            SMOOTHING * deltaTime
        );
    }
}