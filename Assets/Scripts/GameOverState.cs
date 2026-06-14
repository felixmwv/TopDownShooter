using UnityEngine;
using System;

public class GameOverState : IGameState
{
    private readonly Action _onRestart;
    private readonly GameObject _gameOverPanel;

    public GameOverState(Action onRestart, GameObject gameOverPanel)
    {
        _onRestart = onRestart;
        _gameOverPanel = gameOverPanel;
    }

    public void Enter()  => _gameOverPanel.SetActive(true);
    public void Exit()   => _gameOverPanel.SetActive(false);

    public void Tick(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _onRestart?.Invoke();
    }
}