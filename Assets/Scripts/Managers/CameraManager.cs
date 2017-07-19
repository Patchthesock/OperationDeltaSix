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
			MouseLook(_settings.Camera.transform, _settings.MouseLook);
			ClampMovement(_settings.Camera.transform, _settings.Clamp);
            ArrowKeyMovement(_settings.Camera.transform, _settings.FlySpeed);
            MiddleMouseMovement(_settings.Camera.transform, _settings.FlySpeed);
            ScrollWheelMovement(_settings.Camera.transform, _settings.FlySpeed);
        }

        private void MouseLook(Transform camera, Settings.MouseLookSettings mouseLookSettings)
        {
            if (!Input.GetMouseButton(1)) return;
            var originalRotation = camera.localRotation;
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
	                    camera.rotation = originalRotation * xQuaternion * yQuaternion;
					}
					break; 
				case RotationAxes.MouseX:
					{
						rotationX += Input.GetAxis("Mouse X") * mouseLookSettings.SensitivityX;
						rotationX = ClampAngle(rotationX, mouseLookSettings.MinX, mouseLookSettings.MaxX);
						Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
						camera.rotation = originalRotation * xQuaternion;
					}
					break;
				case RotationAxes.MouseY:
					break;
				default:
					{
						rotationY += Input.GetAxis("Mouse Y") * mouseLookSettings.SensitivityY;
						rotationY = ClampAngle(rotationY, mouseLookSettings.MinY, mouseLookSettings.MaxY);
						var yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
						camera.rotation = originalRotation * yQuaternion;
					}
					break;
			}
        }

        private void MiddleMouseMovement(Transform camera, float flySpeed)
        {
            if (!Input.GetMouseButton(2)) return;
			if (Input.GetMouseButtonDown(2))
			{
				_height = camera.transform.position.y;
				_dragOrigin = Input.mousePosition;
			}

			var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin); // TODO: Remove / Do better 
			camera.Translate(new Vector3(pos.x, 0, pos.y) * -1 * flySpeed, Space.Self);
			camera.position = new Vector3(camera.position.x, _height, camera.position.z);
        }







        private static void Move(Transform camera, Vector3 movement, float flySpeed)
        {
            if (Math.Abs(movement.x) > 0.01) camera.Translate(new Vector3(camera.right.x, 0, camera.right.z) * movement.x * flySpeed, Space.World);
            if (Math.Abs(movement.y) > 0.01) camera.Translate(new Vector3(camera.forward.x, 0, camera.forward.z) * movement.y * flySpeed, Space.World);
        }

		private static Vector3 GetArrowKeyMovement()
		{
			return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		}

		private static Vector3 GetScrollWheelMovement(Transform camera, float flySpeed)
		{
			if (Input.GetKey(KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") < 0f) return Vector3.up * flySpeed;
			if (Input.GetKey(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") > 0f) return Vector3.down * flySpeed;
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
