using System;
using Assets.Scripts.Actors;
using Assets.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Controllers
{
    public class CameraController : ITickable
    {
        public CameraController(
            CameraHooks hooks,
            CameraSettings settings,
            IUserCameraInput userInput)
        {
            _hooks = hooks;
            _settings = settings;
            _userInput = userInput;
            _originalRotation = _hooks.transform.rotation;
        }

        public void Tick()
        {
            Clamp(_hooks, _settings);
            MouseLook(_userInput.GetRotationLookInput(), _hooks, _settings);
            MovePositionVertical(_userInput.GetVerticalPositionInput(), _hooks, _settings);
            MovePositionHorizontal(_userInput.GetHorizontalPositionInput(), _hooks, _settings);
        }

        private void MouseLook(Vector2 input, Component hooks, CameraSettings settings)
        {
            switch (settings.axes)
            {
                case RotationAxes.MouseXAndY:
                    {
                        _rotationX += input.x * settings.sensitivityX;
                        _rotationY += input.y * settings.sensitivityY;
                        _rotationX = ClampAngle(_rotationX, settings.minimumX, settings.maximumX);
                        _rotationY = ClampAngle(_rotationY, settings.minimumY, settings.maximumY);
                        var xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
                        var yQuaternion = Quaternion.AngleAxis(_rotationY, -Vector3.right);
                        hooks.transform.rotation = _originalRotation * xQuaternion * yQuaternion;
                    }
                    break;
                case RotationAxes.MouseX:
                    {
                        _rotationX += input.x * settings.sensitivityX;
                        _rotationX = ClampAngle(_rotationX, settings.minimumX, settings.maximumX);
                        var xQuaternion = Quaternion.AngleAxis(_rotationX, Vector3.up);
                        hooks.transform.rotation = _originalRotation * xQuaternion;
                    }
                    break;
                case RotationAxes.MouseY:
                    break;
                default:
                    {
                        _rotationY += input.y * settings.sensitivityY;
                        _rotationY = ClampAngle(_rotationY, settings.minimumY, settings.maximumY);
                        var yQuaternion = Quaternion.AngleAxis(-_rotationY, Vector3.right);
                        hooks.transform.rotation = _originalRotation * yQuaternion;
                    }
                    break;
            }
        }

        private static void MovePositionVertical(float input, Component hooks, CameraSettings settings)
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

        private static void MovePositionHorizontal(Vector2 input, Component hooks, CameraSettings settings)
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
        
        private static void Clamp(Component hooks, CameraSettings settings)
        {
            if (hooks.transform.position.y < settings.MinHeight)
            {
                hooks.transform.position = new Vector3(hooks.transform.position.x, settings.MinHeight, hooks.transform.position.z);
            }

            //if (TheCamera.transform.position.y > MaxHeight)
            //{
            //    TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, MaxHeight, TheCamera.transform.position.z);
            //}

            //if (TheCamera.transform.position.x > MaxMinX)
            //{
            //    TheCamera.transform.position = new Vector3(MaxMinX, TheCamera.transform.position.y, TheCamera.transform.position.z);
            //}

            //if (TheCamera.transform.position.x < -MaxMinX)
            //{
            //    TheCamera.transform.position = new Vector3(-MaxMinX, TheCamera.transform.position.y, TheCamera.transform.position.z);
            //}

            //if (TheCamera.transform.position.z > MaxMinZ)
            //{
            //    TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, TheCamera.transform.position.y, MaxMinZ);
            //}

            //if (TheCamera.transform.position.z < -MaxMinZ)
            //{
            //    TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, TheCamera.transform.position.y, -MaxMinZ);
            //}
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

        private readonly Quaternion _originalRotation;
        private readonly CameraHooks _hooks;
        private readonly CameraSettings _settings;
        private readonly IUserCameraInput _userInput;

        public class CameraSettings
        {
            // Position
            public float MinHeight;
            public float MaxHeight;
            public float MaxMinX;
            public float MaxMinZ;
            public float FlySpeed;
            public float SlowDownRatio;
            public float Acceleration;
            public float AccelerationRatio;

            // Rotation
            public float minimumX = -360f;
            public float maximumX = 360f;
            public float minimumY = -60f;
            public float maximumY = 60f;
            public float sensitivityX = 15f;
            public float sensitivityY = 15f;
            public RotationAxes axes = RotationAxes.MouseXAndY;
        }
    }
}
