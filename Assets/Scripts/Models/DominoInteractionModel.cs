using UnityEngine;

namespace Assets.Scripts.Models
{
    public class DominoInteractionModel
    {
        public readonly Vector3 PositionToApplyForce;
        public readonly Rigidbody Rigidbody;

        public DominoInteractionModel(Vector3 position, Rigidbody rigidbody)
        {
            PositionToApplyForce = new Vector3(position.x, 0, position.z);
            Rigidbody = rigidbody;
        }
    }
}
