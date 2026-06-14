using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuState : IGameState
{
    private readonly Action _onStart;
    private readonly GameObject _menuPanel;

    public MenuState(Action onStart, GameObject menuPanel)
    {
        _onStart = onStart;
        _menuPanel = menuPanel;
    }

    public void Enter()  => _menuPanel.SetActive(true);
    public void Exit()   => _menuPanel.SetActive(false);

    public void Tick(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _onStart?.Invoke();
    }
}