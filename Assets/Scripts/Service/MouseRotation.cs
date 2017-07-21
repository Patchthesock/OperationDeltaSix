using System;
using UnityEngine;

namespace Assets.Scripts.Service
{
    public class MouseRotation
    {
        public MouseRotation(
            Transform model,
            Settings settings
            )
        {
            _model = model;
            _settings = settings;
        }

        public void SetActive(bool state)
        {
            if (!state)
            {
                _rotationX = 0;
                _rotationY = 0;
                _isActive = false;
            }
            else
            {
                _isActive = true;
                _initial = _model.transform.rotation;
            }
        }

        public void Tick()
        {
            if (!_isActive) return;
            var rot = GetMouseLookRotation(_settings).eulerAngles;
            rot.z = 0;
            _model.eulerAngles = rot;
        }

        private Quaternion GetMouseLookRotation(Settings settings)
        {
            _rotationX += Input.GetAxis("Mouse X") * settings.Sensitivity;
            _rotationY += Input.GetAxis("Mouse Y") * settings.Sensitivity;
            _rotationX = ClampAngle(_rotationX, -settings.Speed, settings.Speed);
            _rotationY = ClampAngle(_rotationY, -settings.Speed, settings.Speed);
            return _initial * Quaternion.AngleAxis(_rotationX, Vector3.up) * Quaternion.AngleAxis(_rotationY, -Vector3.right);
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            return Mathf.Clamp(angle, min, max);
        }

        private bool _isActive;
        private float _rotationX;
        private float _rotationY;
        private Quaternion _initial;
        private readonly Transform _model;
        private readonly Settings _settings;

        [Serializable]
        public class Settings
        {
            public float Speed = 0.5f;
            public float Sensitivity = 0.1f;
        }
    }
}
