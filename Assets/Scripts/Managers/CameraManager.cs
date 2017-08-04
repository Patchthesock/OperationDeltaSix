using System;
using Assets.Scripts.Service;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class CameraManager : ITickable
    {
        public CameraManager(Settings settings)
        {
            _settings = settings;
            _mouseRotation = new MouseRotation(_settings.Camera.transform, _settings.Rotation);
        }

        public void SetActive(bool state)
        {
            _isActive = state;
        }

        public Quaternion GetCameraRotation()
        {
            return _settings.Camera.transform.rotation;
        }

        public void Tick()
        {
            if (!_isActive) return;
            GetMiddleMouseVector(_settings.Camera.transform, _settings.FlySpeed);
            _mouseRotation.SetActive(Input.GetMouseButton(1));
            _mouseRotation.Tick();
            Move(_settings.Camera.transform, GetArrowKeyVector() + GetHeightVector(), _settings.FlySpeed, _settings.Clamp);
        }

        private void GetMiddleMouseVector(Transform camera, float flySpeed)
        {
            if (!Input.GetMouseButton(2)) return;
            _dragOrigin = Input.mousePosition;
            var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin); // TODO: Remove / Do better 
            camera.Translate(new Vector3(pos.x, 0, pos.y) * -1 * flySpeed, Space.Self);
        }

        private static Vector3 GetArrowKeyVector()
        {
            return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        }

        private static Vector3 GetHeightVector()
        {
            if (Input.GetKey(KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") < 0f) return Vector3.up;
            if (Input.GetKey(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") > 0f) return Vector3.down;
            return Vector3.zero;
        }

        private static void Move(Transform camera, Vector3 movement, float flySpeed, Settings.ClampSettings settings)
        {
            if (movement.sqrMagnitude < 0.01) return;
            movement = movement * flySpeed;
            if (Math.Abs(movement.y) > 0.01) camera.Translate(new Vector3(0, movement.y, 0), Space.World);
            if (Math.Abs(movement.x) > 0.01) camera.Translate(new Vector3(camera.right.x, 0, camera.right.z) * movement.x, Space.World);
            if (Math.Abs(movement.z) > 0.01) camera.Translate(new Vector3(camera.forward.x, 0, camera.forward.z) * movement.z, Space.World);
            camera.position = ClampVector(camera.position, settings);
        }

        private static Vector3 ClampVector(Vector3 movement, Settings.ClampSettings clampSettings)
        {
            if (movement.x > clampSettings.MaxMinX) movement.x = clampSettings.MaxMinX;
            if (movement.z > clampSettings.MaxMinZ) movement.z = clampSettings.MaxMinZ;
            if (movement.x < -clampSettings.MaxMinX) movement.x = -clampSettings.MaxMinX;
            if (movement.z < -clampSettings.MaxMinZ) movement.z = -clampSettings.MaxMinZ;
            if (movement.y < clampSettings.MinHeight) movement.y = clampSettings.MinHeight;
            if (movement.y > clampSettings.MaxHeight) movement.y = clampSettings.MaxHeight;
            return movement;
        }

        private bool _isActive;        
        private Vector3 _dragOrigin;
        private readonly Settings _settings;
        private readonly MouseRotation _mouseRotation;

        [Serializable]
        public class Settings
        {
            public float FlySpeed;
            public GameObject Camera;
            public ClampSettings Clamp;
            public MouseRotation.Settings Rotation;

            [Serializable]
            public class ClampSettings
            {
                public float MaxMinX;
                public float MaxMinZ;
                public float MinHeight;
                public float MaxHeight;
            }
        }
    }
}
