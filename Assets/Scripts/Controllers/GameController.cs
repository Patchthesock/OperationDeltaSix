using Assets.Scripts.Factories;
//using Assets.Scripts.States.Game;
using Zenject;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class GameController : ITickable
    {
        public GameController(
            //GameStateFactory stateFactory,
            LevelController levelController,
            CameraController cameraController,
            DominoController dominoController,
            GameStateController gameStateController,
            PlacementController placementController)
        {
           // _stateFactory = stateFactory;
            _levelController = levelController;
            _cameraController = cameraController;
            _dominoController = dominoController;
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
        //    _currentState.Update();
        }

        private void ChangeState(GameStateFactory.GameStates state)
        {
            //if (_currentState != null) _currentState.Stop();
            //_currentState = _stateFactory.Create(state, this);
            //_currentState.Start();
            if (state == GameStateFactory.GameStates.Build)
            {
                Physics.gravity = new Vector3(0, 0, 0);
                _dominoController.SetPhysics(false);

            }
            else
            {
                Physics.gravity = new Vector3(0, -50, 0);
                _dominoController.SetPhysics(true);
                
            }
        }
        

       // private GameState _currentState;
       // private readonly GameStateFactory _stateFactory;
        private readonly LevelController _levelController;
        private readonly CameraController _cameraController;
        private readonly DominoController _dominoController;
        private readonly GameStateController _gameStateController;
        private readonly PlacementController _placementController;
    }
}
