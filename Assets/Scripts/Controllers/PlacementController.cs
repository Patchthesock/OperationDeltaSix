using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class PlacementController
    {
        public PlacementController(
            Settings settings,
            MenuController menuController,
            GhostController ghostController,
            InputController inputController,
            CameraController cameraController)
        {
            _settings = settings;
            _menuController = menuController;
            _ghostController = ghostController;
            _inputController = inputController;
            _cameraController = cameraController;
        }

        public void Initialize()
        {
            _menuController.Initialize();
            _menuController.SubscribeToOnItemSelected(OnMenuItemSelected);
        }

        private void OnMenuItemSelected(GameObject model)
        {
            Debug.Log("Fired");
            _ghostController.Select(model);
        }

        private Quaternion GetSingleDominoRotation(Vector3 pos)
        {
            if (Time.time - _timeLastPlaced <= _settings.TimeToLine)
            {
                var rot = Quaternion.LookRotation(_positionLastPlaced - pos);
                _positionLastPlaced = pos;
                return rot;
            }
            _positionLastPlaced = pos;
            _timeLastPlaced = Time.time;
            return GetDefaultSingleDominoRotation();
        }

        private Quaternion GetDefaultSingleDominoRotation()
        {
            var rotation = _cameraController.Rotation.eulerAngles;
            rotation = new Vector3(0, rotation.y, rotation.z);
            return Quaternion.Euler(rotation);
        }

        private float _timeLastPlaced;
        private Vector3 _positionLastPlaced;
        private readonly Settings _settings;
        private readonly MenuController _menuController;
        private readonly GhostController _ghostController;
        private readonly InputController _inputController;
        private readonly CameraController _cameraController;

        [Serializable]
        public class Settings
        {
            public float TimeToLine;
        }
    }
}
