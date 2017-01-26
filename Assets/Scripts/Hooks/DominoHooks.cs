using UnityEngine;

namespace Assets.Scripts.Hooks
{
    public class DominoHooks : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;

        public Collider Collider
        {
            get { return _collider; }
        }

        public Rigidbody Rigidbody
        {
            get { return _rigidbody; }
        }
    }
}
