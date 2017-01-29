using System;
using Assets.Scripts.Actors;
using Assets.Scripts.Factories;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Controllers
{
    public class CameraController : ITickable
    {
        public CameraController(
            Settings settings,
            PrefabFactory prefabFactory,
            InputController inputController)
        {
            _settings = settings;
            _prefabFactory = prefabFactory;
            _inputController = inputController;
        }

        public void Initialize()
        {
            _camera = new PlayerCamera(_prefabFactory.Create(_settings.Camera).GetComponent<CameraHooks>());
            _camera.Transform.position = _settings.InitialPosition;
            _camera.Transform.rotation = Quaternion.Euler(_settings.InitialRotaiton);
            _originalRotation = Quaternion.Euler(_settings.InitialRotaiton);
        }

        public void Tick()
        {
            if (_camera == null) return;
            Clamp(_camera.Transform, _settings);
            MouseLook(_inputController.GetRotationLookInput(), _camera.Transform, _settings);
            MovePositionVertical(_inputController.GetVerticalPositionInput(), _camera.Transform, _settings);
            MovePositionHorizontal(_inputController.GetHorizontalPositionInput(), _camera.Transform, _settings);
        }

        public Ray ScreenPointToRay(Vector3 position)
        {
            return _camera.Camera.ScreenPointToRay(position);
        }

        public Vector3 Position
        {
            get { return _camera.Transform.position; }
        }

        public Quaternion Rotation
        {
            get { return _camera.Transform.rotation; }
        }

        private void MouseLook(Vector2 input, Component hooks, Settings settings)
        {
            switch (settings.Axes)
            {
                case RotationAxes.MouseXAndY:
                {
                    _rotationX += input.x * settings.SensitivityX;
                    _rotationY += input.y * settings.SensitivityY;
                    _rotationX = ClampAngle(_rotationX, settings.MinimumX, settings.MaximumX);
                    _rotationY = ClampAngle(_rotationY, settings.MinimumY, settings.MaximumY);
                    var xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
                    var yQuaternion = Quaternion.AngleAxis(_rotationY, -Vector3.right);
                    hooks.transform.rotation = _originalRotation * xQuaternion * yQuaternion;
                    break;
                }
                    
                case RotationAxes.MouseX:
                {
                    _rotationX += input.x * settings.SensitivityX;
                    _rotationX = ClampAngle(_rotationX, settings.MinimumX, settings.MaximumX);
                    var xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
                    hooks.transform.rotation = _originalRotation * xQuaternion;
                    break;
                }
                    
                case RotationAxes.MouseY:
                {
                    break;
                }
                    
                default:
                {
                    _rotationY += input.y * settings.SensitivityY;
                    _rotationY = ClampAngle(_rotationY, settings.MinimumY, settings.MaximumY);
                    var yQuaternion = Quaternion.AngleAxis(-_rotationY, Vector3.right);
                    hooks.transform.rotation = _originalRotation * yQuaternion;
                    break;
                }  
            }
        }

        private static void MovePositionVertical(float input, Component hooks, Settings settings)
        {
            if (input < 0f)
            {
                hooks.transform.position = hooks.transform.position + Vector3.up * settings.FlySpeed;
            }

            if (input > 0f)
            {
                hooks.transform.position = hooks.transform.position + Vector3.down * settings.FlySpeed;
            }
        }

        private static void MovePositionHorizontal(Vector2 input, Component hooks, Settings settings)
        {
            if (Math.Abs(input.y) > 0.01)
            {
                hooks.transform.Translate(
                    new Vector3(hooks.transform.forward.x, 0, hooks.transform.forward.z) * input.y * settings.FlySpeed, Space.World);
            }

            if (Math.Abs(input.x) > 0.01)
            {
                hooks.transform.Translate(
                    new Vector3(hooks.transform.right.x, 0, hooks.transform.right.z) * input.x * settings.FlySpeed, Space.World);
            }
        }
        
        private static void Clamp(Component hooks, Settings settings)
        {
            if (hooks.transform.position.y < settings.MinHeight)
            {
                hooks.transform.position = new Vector3(hooks.transform.position.x, settings.MinHeight, hooks.transform.position.z);
            }

            if (hooks.transform.position.y > settings.MaxHeight)
            {
                hooks.transform.position = new Vector3(hooks.transform.position.x, settings.MaxHeight, hooks.transform.position.z);
            }

            if (hooks.transform.position.x > settings.MaxMinX)
            {
                hooks.transform.position = new Vector3(settings.MaxMinX, hooks.transform.position.y, hooks.transform.position.z);
            }

            if (hooks.transform.position.x < -settings.MaxMinX)
            {
                hooks.transform.position = new Vector3(-settings.MaxMinX, hooks.transform.position.y, hooks.transform.position.z);
            }

            if (hooks.transform.position.z > settings.MaxMinZ)
            {
                hooks.transform.position = new Vector3(hooks.transform.position.x, hooks.transform.position.y, settings.MaxMinZ);
            }

            if (hooks.transform.position.z < -settings.MaxMinZ)
            {
                hooks.transform.position = new Vector3(hooks.transform.position.x, hooks.transform.position.y, -settings.MaxMinZ);
            }
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

        public enum RotationAxes
        {
            MouseXAndY = 0,
            MouseX = 1,
            MouseY = 2
        }

        private float _rotationX;
        private float _rotationY;
        private PlayerCamera _camera;
        private Quaternion _originalRotation;

        private readonly Settings _settings;
        private readonly InputController _inputController;
        private readonly PrefabFactory _prefabFactory;

        [Serializable]
        public class Settings
        {
            // Init
            public Vector3 InitialPosition;
            public Vector3 InitialRotaiton;

            // Position
            public float MaxMinX;
            public float MaxMinZ;
            public float FlySpeed;
            public float MinHeight;
            public float MaxHeight;

            // Rotation
            public float MinimumX = -360f;
            public float MaximumX = 360f;
            public float MinimumY = -60f;
            public float MaximumY = 60f;
            public float SensitivityX = 15f;
            public float SensitivityY = 15f;
            public RotationAxes Axes = RotationAxes.MouseXAndY;

            // GameObject
            public GameObject Camera;
        }
    }
}
