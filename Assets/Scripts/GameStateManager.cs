public class GameStateManager
{
    private IGameState _currentState;

    public void SwitchState(IGameState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Tick(float deltaTime) => _currentState?.Tick(deltaTime);
}
