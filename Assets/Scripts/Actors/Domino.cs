using Assets.Scripts.Hooks;

namespace Assets.Scripts.Actors
{
    public class Domino
    {
        public Domino(DominoHooks hooks)
        {
            _hooks = hooks;
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
