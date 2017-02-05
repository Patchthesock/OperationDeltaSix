using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Controllers
{
    public class InteractController : ITickable
    {
        public InteractController(
            Settings settings,
            InputController inputController)
        {
            _isReady = false;
            _settings = settings;
            _inputController = inputController;
        }

        public void SetState(bool isReady)
        {
            _isReady = isReady;
        }

        public void Tick()
        {
            if (_isReady) CheckInteract();
        }

        private void CheckInteract()
        {
            var domino = _inputController.GetMouseClickDomino(0);
            if (domino == null) return;
            domino.Rigidbody
                .AddForceAtPosition(
                    -domino.PositionToApplyForce.normalized * _settings.DominoPushForce,
                    domino.PositionToApplyForce,
                    ForceMode.Impulse);
        }

        private bool _isReady;
        private readonly Settings _settings;
        private readonly InputController _inputController;

        [Serializable]
        public class Settings
        {
            public float DominoPushForce;
        }
    }
}
