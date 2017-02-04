using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class InputController
    {
        public bool HasMouseLook()
        {
            return Input.GetMouseButton(1);
        }

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

        public Vector3 GetMousePosition()
        {
            var hit = GetRaycastHit(Input.mousePosition);
            if (hit.collider == null) return new Vector3();
            return hit.collider.gameObject.tag != "Ground" ? Vector3.zero : hit.point + new Vector3(0, 0.6f, 0);
        }

        public Vector3 GetMouseClickPosition(int mouseButtonNumber)
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return new Vector3();
            if (!Input.GetMouseButton(mouseButtonNumber)) return Vector3.zero;
            var hit = GetRaycastHit(Input.mousePosition);
            if (hit.collider == null) return new Vector3();
            return hit.collider.gameObject.tag != "Ground" ? Vector3.zero : hit.point;
        }

        public string GetMouseClickItemName(int mouseButtonNumber)
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return string.Empty;
            if (!Input.GetMouseButton(mouseButtonNumber)) return string.Empty;
            var hit = GetRaycastHit(Input.mousePosition);
            if (hit.collider == null) return string.Empty;
            return hit.collider.gameObject.tag != "Domino" ? string.Empty : hit.collider.gameObject.name;
        }

        private static RaycastHit GetRaycastHit(Vector3 position)
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(position);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            return hit;
        }
    }
}
