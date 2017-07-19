using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class CameraManager : ITickable
    {
        public CameraManager(Settings settings)
        {
            _settings = settings;
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
            Rotate(_settings.Camera.transform, GetMouseLookVector(_settings.MouseLook));
            Move(_settings.Camera.transform, GetArrowKeyVector() + GetScrollWheelVector(), _settings.FlySpeed, _settings.Clamp);         
        }

        private void GetMiddleMouseVector(Transform camera, float flySpeed)
        {
            if (!Input.GetMouseButton(2)) return;
            _height = camera.transform.position.y;
            _dragOrigin = Input.mousePosition;
            var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin); // TODO: Remove / Do better 
            camera.Translate(new Vector3(pos.x, 0, pos.y) * -1 * flySpeed, Space.Self);
            camera.position = new Vector3(camera.position.x, _height, camera.position.z);
        }

        private static void Move(Transform camera, Vector3 movement, float flySpeed, Settings.ClampSettings settings)
        {
            if (Math.Abs(movement.x) > 0.01) camera.Translate(new Vector3(camera.right.x, 0, camera.right.z) * movement.x * flySpeed, Space.World);
            if (Math.Abs(movement.z) > 0.01) camera.Translate(new Vector3(camera.forward.x, 0, camera.forward.z) * movement.z * flySpeed, Space.World);
            camera.position = ClampVector(camera.position, settings);
        }

        private static void Rotate(Transform camera, Vector3 rotation)
        {
            if (rotation == Vector3.zero) return;
            rotation.z = 0;
            camera.eulerAngles = rotation;
        }

        private Vector3 GetMouseLookVector(Settings.MouseLookSettings mouseLookSettings)
        {
            if (!Input.GetMouseButton(1)) return Vector3.zero;
            _rotationX += Input.GetAxis("Mouse X") * mouseLookSettings.SensitivityX;
            _rotationY += Input.GetAxis("Mouse Y") * mouseLookSettings.SensitivityY;
            var xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
            var yQuaternion = Quaternion.AngleAxis(_rotationY, -Vector3.right);
            return (xQuaternion * yQuaternion).eulerAngles;
        }

        private static Vector3 GetArrowKeyVector()
		{
			return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		}

		private static Vector3 GetScrollWheelVector()
		{
			if (Input.GetKey(KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") < 0f) return Vector3.up;
			if (Input.GetKey(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") > 0f) return Vector3.down;
			return Vector3.zero;
		}

		private static float ClampAngle(float angle, float min, float max)
		{
            if (angle > 360F) angle -= 360F;
			if (angle < -360F) angle += 360F;
			return Mathf.Clamp(angle, min, max);
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

		private float _height;
        private bool _isActive;
        private float _rotationX;
        private float _rotationY;
        private Vector3 _dragOrigin;
        private readonly Settings _settings;

        [Serializable]
        public class Settings
        {
			public float FlySpeed;
            public GameObject Camera;
            public ClampSettings Clamp;
            public MouseLookSettings MouseLook;

            [Serializable]
            public class ClampSettings
            {
				public float MinHeight;
				public float MaxHeight;
				public float MaxMinX;
				public float MaxMinZ;
            }

            [Serializable]
            public class MouseLookSettings
            {
				public float MinX = -360F;
				public float MaxX = 360F;
				public float MinY = -60F;
				public float MaxY = 60F;
				public float SensitivityX = 15F;
				public float SensitivityY = 15F;
            }
        }
    }
}
