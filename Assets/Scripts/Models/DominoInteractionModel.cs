﻿using UnityEngine;

namespace Assets.Scripts.Models
{
    public class DominoInteractionModel
    {
        public readonly Vector3 PositionToApplyForce;
        public readonly Rigidbody Rigidbody;

        public DominoInteractionModel(Vector3 position, Rigidbody rigidbody)
        {
            PositionToApplyForce = position;
            Rigidbody = rigidbody;
        }
    }
}
