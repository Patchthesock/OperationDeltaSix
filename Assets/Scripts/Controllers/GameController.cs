using Assets.Scripts.Factories;
using Assets.Scripts.States.Game;
using Zenject;

namespace Assets.Scripts.Controllers
{
    public class GameController : ITickable
    {
        public GameController(
            GameStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public void Initialize()
        {
            ChangeState(GameStateFactory.GameStates.Build);
        }

        public void Tick()
        {
            _currentState.Update();
        }

        private void ChangeState(GameStateFactory.GameStates state)
        {
            if (_currentState != null) _currentState.Stop();
            _currentState = _stateFactory.Create(state, this);
            _currentState.Start();
        }

        private GameState _currentState;
        private readonly GameStateFactory _stateFactory;
    }
}
