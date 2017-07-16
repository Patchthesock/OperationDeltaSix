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
            Debug.Log("I'm active");
        }

        private bool _isActive;
    }
}
