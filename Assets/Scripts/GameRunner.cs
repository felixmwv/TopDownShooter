using UnityEngine;

public class GameRunner : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;

    private PlayerSystem _player;

    private void Start()
    {
        var rb = _playerObject.GetComponent<Rigidbody2D>();
        _player = new PlayerSystem(_playerObject.transform, rb);
    }

    private void Update()
    {
        InputData input = new InputData
        {
            MoveDirection = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            ),
            MouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition)
        };

        _player.Tick(Time.deltaTime, input);
    }
}