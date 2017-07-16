using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class DominoInteractionManager : ITickable
    {
        public DominoInteractionManager(Settings settings)
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
            if (!Functions.GetMouseButtonInput(0)) return;
            var hit = Functions.GetHit();
            if (hit.collider.gameObject.tag != "Domino") return;
            hit.collider.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal*_settings.KnockForce, hit.point, ForceMode.Impulse);
        }

        private bool _isActive;
        private readonly Settings _settings;

        [Serializable]
        public class Settings
        {
            public float KnockForce;
        }
    }
}
