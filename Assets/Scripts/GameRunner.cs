using UnityEngine;

public class GameRunner : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _bulletPoolSize = 20;

    private PlayerSystem _player;
    private BulletPool _bulletPool;
    private ShootingSystem _shooting;

    private void Start()
    {
        var rb = _playerObject.GetComponent<Rigidbody2D>();
        _player = new PlayerSystem(_playerObject.transform, rb);

        _bulletPool = new BulletPool(CreateBulletTransforms());
        _shooting = new ShootingSystem(_bulletPool, _playerObject.transform);
    }

    private void Update()
    {
        InputData input = new InputData
        {
            MoveDirection = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ),
            MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition),
            ShootPressed = Input.GetMouseButton(0)
        };

        _player.Tick(Time.deltaTime, input);
        _shooting.Tick(Time.deltaTime, input);
        _bulletPool.Tick(Time.deltaTime);
    }

    private Transform[] CreateBulletTransforms()
    {
        Transform[] transforms = new Transform[_bulletPoolSize];

        for (int i = 0; i < _bulletPoolSize; i++)
            transforms[i] = Instantiate(_bulletPrefab).transform;

        return transforms;
    }
}