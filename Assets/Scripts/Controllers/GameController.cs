using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class GameController
    {
        public GameController(
            CameraController cameraController,
            DominoController dominoController,
            InteractController interactController,
            GameStateController gameStateController,
            PlacementController placementController)
        {
            _cameraController = cameraController;
            _dominoController = dominoController;
            _interactController = interactController;
            _gameStateController = gameStateController;
            _placementController = placementController;
        }

        public void Initialize()
        {
            _cameraController.Initialize();
            _placementController.Initialize();
            _gameStateController.SubscribeToOnGameStateChanged(ChangeState);
            _gameStateController.Initialize(false);
        }

        private void ChangeState(bool isPlaying)
        {
            _dominoController.SetPhysics(isPlaying);
            _interactController.SetState(isPlaying);
            Physics.gravity = isPlaying ? new Vector3(0, -50, 0) : new Vector3(0, 0, 0);
        }
        
        private readonly CameraController _cameraController;
        private readonly DominoController _dominoController;
        private readonly InteractController _interactController;
        private readonly GameStateController _gameStateController;
        private readonly PlacementController _placementController;
    }
}
