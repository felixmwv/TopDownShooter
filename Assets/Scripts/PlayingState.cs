using System;

public class PlayingState : IGameState
{
    private readonly Action _onGameOver;
    private readonly PlayerSystem _player;
    private readonly EnemyManager _enemyManager;

    public PlayingState(PlayerSystem player, EnemyManager enemyManager, Action onGameOver)
    {
        _player = player;
        _enemyManager = enemyManager;
        _onGameOver = onGameOver;
    }

    public void Enter()  { }
    public void Exit()   { }

    public void Tick(float deltaTime)
    {
        if (_player.IsDead)
            _onGameOver?.Invoke();
    }
}