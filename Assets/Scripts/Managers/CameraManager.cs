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

        public void Tick()
        {
            if (!_isActive) return;
            ClampMovement(_settings.Camera);
            ArrowKeyMovement(_settings.Camera);
            MiddleMouseMovement(_settings.Camera);
            ScrollWheelMovement(_settings.Camera);
            MouseLook(_settings.Camera, _settings.MouseLook);
        }

        private void MouseLook(GameObject camera, Settings.MouseLookSettings mouseLookSettings)
        {
            if (!Input.GetMouseButton(1)) return;
            var originalRotation = _settings.Camera.transform.localRotation;
			switch (mouseLookSettings.Axes)
			{
				case RotationAxes.MouseXAndY:
					{
						rotationX += Input.GetAxis("Mouse X") * mouseLookSettings.SensitivityX;
						rotationY += Input.GetAxis("Mouse Y") * mouseLookSettings.SensitivityY;
						rotationX = ClampAngle(rotationX, mouseLookSettings.MinX, mouseLookSettings.MaxX);
						rotationY = ClampAngle(rotationY, mouseLookSettings.MinY, mouseLookSettings.MaxY);
						var xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
						var yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
	                    camera.transform.rotation = originalRotation * xQuaternion * yQuaternion;
					}
					break; 
				case RotationAxes.MouseX:
					{
						rotationX += Input.GetAxis("Mouse X") * mouseLookSettings.SensitivityX;
						rotationX = ClampAngle(rotationX, mouseLookSettings.MinX, mouseLookSettings.MaxX);
						Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
						camera.transform.rotation = originalRotation * xQuaternion;
					}
					break;
				case RotationAxes.MouseY:
					break;
				default:
					{
						rotationY += Input.GetAxis("Mouse Y") * mouseLookSettings.SensitivityY;
						rotationY = ClampAngle(rotationY, mouseLookSettings.MinY, mouseLookSettings.MaxY);
						var yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
						camera.transform.rotation = originalRotation * yQuaternion;
					}
					break;
			}
        }

        private void MiddleMouseMovement(GameObject camera)
        {
            if (!Input.GetMouseButton(2)) return;
			if (Input.GetMouseButtonDown(2))
			{
				_height = camera.transform.position.y;
				_dragOrigin = Input.mousePosition;
			}

			var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
			camera.transform.Translate(new Vector3(pos.x, 0, pos.y) * -1 * FlySpeed, Space.Self);
			camera.transform.position = new Vector3(camera.transform.position.x, _height, camera.transform.position.z);
        }

        private void ArrowKeyMovement(GameObject camera)
        {
			if (Math.Abs(Input.GetAxis("Vertical")) > 0.01) camera.transform.Translate(new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z) * Input.GetAxis("Vertical") * FlySpeed, Space.World);
			if (Math.Abs(Input.GetAxis("Horizontal")) > 0.01) camera.transform.Translate(new Vector3(camera.transform.right.x, 0, camera.transform.right.z) * Input.GetAxis("Horizontal") * FlySpeed, Space.World);
        }

        private void ScrollWheelMovement(GameObject camera)
        {
			if (Input.GetKey(KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") < 0f) camera.transform.position = camera.transform.position + Vector3.up * FlySpeed;
			if (Input.GetKey(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") > 0f) camera.transform.position = camera.transform.position + Vector3.down * FlySpeed;
        }

        private void ClampMovement(GameObject camera)
        {
            if (camera.transform.position.x > MaxMinX) camera.transform.position = new Vector3(MaxMinX, camera.transform.position.y, camera.transform.position.z);
            if (camera.transform.position.z > MaxMinZ) camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, MaxMinZ);
            if (camera.transform.position.x < -MaxMinX) camera.transform.position = new Vector3(-MaxMinX, camera.transform.position.y, camera.transform.position.z);
            if (camera.transform.position.z < -MaxMinZ) camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -MaxMinZ);
            if (camera.transform.position.y < MinHeight) camera.transform.position = new Vector3(camera.transform.position.x, MinHeight, camera.transform.position.z);
            if (camera.transform.position.y > MaxHeight) camera.transform.position = new Vector3(camera.transform.position.x, MaxHeight, camera.transform.position.z);
        }

		private static float ClampAngle(float angle, float min, float max)
		{
            if (angle > 360F) angle -= 360F;
			if (angle < -360F) angle += 360F;
			return Mathf.Clamp(angle, min, max);
		}

		private float _height;
        private bool _isActive;
		private float rotationX;
		private float rotationY;
		private Vector3 _dragOrigin;
        private Quaternion _originalRotation;
        private readonly Settings _settings;

		public enum RotationAxes
		{
			MouseXAndY = 0,
			MouseX = 1,
			MouseY = 2
		}

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
                public RotationAxes Axes = RotationAxes.MouseXAndY;
            }
        }
    }
}
