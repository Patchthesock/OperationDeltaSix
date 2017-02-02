using Assets.Scripts.Factories;
using Assets.Scripts.States.Game;
using Zenject;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class GameController : ITickable
    {
        public GameController(
            GameStateFactory stateFactory,
            LevelController levelController,
            CameraController cameraController,
            GameStateController gameStateController,
            PlacementController placementController)
        {
            _stateFactory = stateFactory;
            _levelController = levelController;
            _cameraController = cameraController;
            _gameStateController = gameStateController;
            _placementController = placementController;
        }

        public void Initialize()
        {
            _levelController.Initialize();
            _cameraController.Initialize();
            _placementController.Initialize();
            _gameStateController.SubscribeToOnGameStateChanged(ChangeState);
            _gameStateController.Initialize(GameStateFactory.GameStates.Build);
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
            Debug.Log(_currentState.ToString());
        }

        private GameState _currentState;
        private readonly GameStateFactory _stateFactory;
        private readonly LevelController _levelController;
        private readonly CameraController _cameraController;
        private readonly GameStateController _gameStateController;
        private readonly PlacementController _placementController;
    }
}
