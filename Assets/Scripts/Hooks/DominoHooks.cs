using UnityEngine;

namespace Assets.Scripts.Hooks
{
    public class DominoHooks : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _transform;
        [SerializeField] private Rigidbody _rigidbody;

        public Collider Collider
        {
            get { return _collider; }
        }

        public Transform Transform
        {
            get { return _transform; }
        }

        public Rigidbody Rigidbody
        {
            get { return _rigidbody; }
        }
    }
}
