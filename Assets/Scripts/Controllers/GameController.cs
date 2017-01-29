using Assets.Scripts.Factories;
using Assets.Scripts.States.Game;
using Zenject;

namespace Assets.Scripts.Controllers
{
    public class GameController : ITickable
    {
        public GameController(
            GameStateFactory stateFactory,
            LevelController levelController,
            CameraController cameraController,
            DominoController dominoController,
            PlacementController placementController)
        {
            _stateFactory = stateFactory;
            _levelController = levelController;
            _cameraController = cameraController;
            _dominoController = dominoController;
            _placementController = placementController;
        }

        public void Initialize()
        {
            _levelController.Initialize();
            _cameraController.Initialize();
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
        private readonly LevelController _levelController;
        private readonly CameraController _cameraController;
        private readonly DominoController _dominoController;
        private readonly PlacementController _placementController;
        
    }
}
