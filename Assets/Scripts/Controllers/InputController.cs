using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class InputController
    {
        public Vector2 GetRotationLookInput()
        {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        public float GetVerticalPositionInput()
        {
            var d = Input.GetAxis("Mouse ScrollWheel");
            if (Input.GetKey(KeyCode.E) || d < 0f)
            {
                return 1;
            }
            if (Input.GetKey(KeyCode.Q) || d > 0f)
            {
                return -1;
            }
            return 0;
        }

        public Vector2 GetHorizontalPositionInput()
        {
            if (Math.Abs(Input.GetAxis("Vertical")) > 0.01 || Math.Abs(Input.GetAxis("Horizontal")) > 0.01)
            {
                 return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }
            return Vector2.zero;
        } 
    }
}
