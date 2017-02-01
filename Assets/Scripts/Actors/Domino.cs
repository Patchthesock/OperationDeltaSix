using Assets.Scripts.Hooks;
using UnityEngine;

namespace Assets.Scripts.Actors
{
    public class Domino
    {
        public Domino(string name, DominoHooks hooks)
        {
            _hooks = hooks;
            _hooks.gameObject.name = name;
        }

        public string Name
        {
            get { return _hooks.gameObject.name; } 
        }

        public Transform Transform
        {
            get { return _hooks.Transform; }
        }

        public void SetPhysics(bool state)
        {
            _hooks.gameObject.layer = state ? 1 : 2;
            if (_hooks.Collider != null)
            {
                _hooks.Collider.isTrigger = !state;
            }
            if (_hooks.Rigidbody == null) return;
            _hooks.Rigidbody.useGravity = state;
            _hooks.Rigidbody.isKinematic = !state;
        }

        private readonly DominoHooks _hooks;
    }
}
