using System;
using UnityEngine;

namespace Assets.Scripts.Service
{
    public class MouseRotation
    {
        public MouseRotation(
            Transform model,
            Settings settings)
        {
            _model = model;
            _settings = settings;
        }

        public void SetActive(bool state)
        {
            if (state) Activate();
            else Reset();
        }

        public void Tick()
        {
            if (!_isActive) return;
            _model.eulerAngles = GetMouseLookRotation(_settings);
        }

        private void Activate()
        {
            _isActive = true;
            _initial = _model.transform.rotation.eulerAngles;
        }

        private void Reset()
        {
            _rotationX = 0;
            _rotationY = 0;
            _isActive = false;
        }

        private Vector3 GetMouseLookRotation(Settings settings)
        {
            _rotationX += Input.GetAxis("Mouse X") * settings.Sensitivity;
            _rotationY += Input.GetAxis("Mouse Y") * settings.Sensitivity;
            var x = _initial + new Vector3(-_rotationY, _rotationX, 0);
            x.x = ClampAngle(x.x, -50, 50);
            return x;
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle > 100) angle -= 360;
            if (angle < -360) angle += 360;
            return Mathf.Clamp(angle, min, max);
        }

        private bool _isActive;
        private float _rotationX;
        private float _rotationY;
        private Vector3 _initial;
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
