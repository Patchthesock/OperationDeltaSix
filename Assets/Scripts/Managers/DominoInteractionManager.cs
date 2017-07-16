using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class DominoInteractionManager : ITickable
    {
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
            hit.collider.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal*100, hit.point, ForceMode.Impulse);
        }

        private bool _isActive;
    }
}
