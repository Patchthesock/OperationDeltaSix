using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Actors
{
    public class PlayerCamera
    {
        public PlayerCamera(CameraHooks hooks)
        {
            _hooks = hooks;
        }

        public Camera Camera
        {
            get { return _hooks.Camera; }
        }

        public Transform Transform
        {
            get { return _hooks.transform; }
        }

        private readonly CameraHooks _hooks;
    }
}
