using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameRunner : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;
    [SerializeField] private GameObject _fireObject;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _basicEnemyPrefab;
    [SerializeField] private GameObject _fastEnemyPrefab;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _gameOverScoreText;
    [SerializeField] private int _bulletPoolSize = 20;
    [SerializeField] private int _enemyPoolSize = 15;
    [SerializeField] private GameObject _background;
    
    private float _playerRadius;
    
    private PlayerSystem _player;
    private BulletPool _bulletPool;
    private ShootingSystem _shooting;
    private EnemyManager _enemyManager;
    private GameStateManager _stateManager;
    private ScoreSystem _score;
    private ParallaxSystem _parallax;
    private MenuState _menuState;
    private PlayingState _playingState;
    private GameOverState _gameOverState;
    private bool _isPlaying;

    private void Start()
    {
        Camera cam = Camera.main;
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        Vector2 halfBounds = new Vector2(halfWidth, halfHeight);

        var rb = _playerObject.GetComponent<Rigidbody2D>();
        _player = new PlayerSystem(_playerObject.transform, rb, halfBounds, 
            _fireObject != null ? _fireObject.transform : null);
        _playerRadius = GetSpriteRadius(_playerObject, multiplier: 0.5f);
        
        _bulletPool = new BulletPool(CreateBulletTransforms(), _playerObject.transform);
        _shooting = new ShootingSystem(_bulletPool, _playerObject.transform);

        var factory = new EnemyFactory(_basicEnemyPrefab, _fastEnemyPrefab);
        _enemyManager = new EnemyManager(factory, _enemyPoolSize);

        _score = new ScoreSystem();
        _stateManager = new GameStateManager();

        _menuState = new MenuState(StartGame, _menuPanel);
        _playingState = new PlayingState(_player, _enemyManager, GameOver);
        _gameOverState = new GameOverState(RestartGame, _gameOverPanel);
        
        _parallax = new ParallaxSystem(_background.transform);
        
        _stateManager.SwitchState(_menuState);
    }

    private void Update()
    {
        _stateManager.Tick(Time.deltaTime);
        _parallax.Tick(Time.deltaTime, _playerObject.transform.position);
        
        if (!_isPlaying) return;
        
        Vector2 playerPosition = _playerObject.transform.position;

        InputData input = new InputData
        {
            MoveDirection = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ),
            MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition),
            ShootPressed = Input.GetMouseButton(0),
            BoostPressed = Input.GetKey(KeyCode.LeftShift)
        };

        _player.Tick(Time.deltaTime, input);
        _shooting.Tick(Time.deltaTime, input);
        _bulletPool.Tick(Time.deltaTime);
        _enemyManager.Tick(Time.deltaTime, playerPosition);

        CheckBulletEnemyCollisions();
        CheckPlayerEnemyCollisions();

        _scoreText.text = $"Score: {_score.Score}";
    }

    private void StartGame()
    {
        _isPlaying = true;
        _stateManager.SwitchState(_playingState);
    }

    private void GameOver()
    {
        _isPlaying = false;
        _gameOverScoreText.text = $"Score: {_score.Score}";
        _stateManager.SwitchState(_gameOverState);
    }

    private void RestartGame()
    {
        _score.Reset();
        _player.Reset(Vector2.zero);
        _enemyManager.DeactivateAll();
        StartGame();
    }

    private void CheckBulletEnemyCollisions()
    {
        foreach (Enemy enemy in _enemyManager.GetEnemies())
        {
            if (!enemy.IsActive) continue;

            foreach (Bullet bullet in _bulletPool.GetBullets())
            {
                if (!bullet.IsActive) continue;

                float distance = Vector2.Distance(bullet.Transform.position, enemy.Transform.position);
                if (distance < bullet.ColliderRadius + enemy.ColliderRadius)
                {
                    bool killed = enemy.TakeDamage();
                    if (killed) _score.AddPoint();
                    _bulletPool.Deactivate(bullet);
                }
            }
        }
    }

    private void CheckPlayerEnemyCollisions()
    {
        Vector2 playerPos = _playerObject.transform.position;

        foreach (Enemy enemy in _enemyManager.GetEnemies())
        {
            if (!enemy.IsActive) continue;

            float distance = Vector2.Distance(playerPos, enemy.Transform.position);
            if (distance < _playerRadius + enemy.ColliderRadius)
            {
                _player.TakeDamage(999f);
                return;
            }
        }
    }
    
    private float GetSpriteRadius(GameObject obj, float multiplier = 1f)
    {
        var sr = obj.GetComponent<SpriteRenderer>();
        if (sr == null) return 0.25f;
        
        return Mathf.Min(sr.bounds.extents.x, sr.bounds.extents.y) * multiplier;
    }
    
    private Transform[] CreateBulletTransforms()
    {
        Transform[] transforms = new Transform[_bulletPoolSize];
        for (int i = 0; i < _bulletPoolSize; i++)
            transforms[i] = Instantiate(_bulletPrefab).transform;
        return transforms;
    }
}