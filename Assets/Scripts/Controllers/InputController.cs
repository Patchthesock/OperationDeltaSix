using System;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class InputController
    {
        public InputController(
            CameraController cameraController)
        {
            _cameraController = cameraController;
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
            var hit = GetRaycastHit();
            if (hit.collider == null) return new Vector3();
            return hit.collider.gameObject.tag != "Ground" ? new Vector3() : hit.point + new Vector3(0, 0.6f, 0);
        }

        public Vector3 GetMouseClickPosition(int mouseButtonNumber)
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return new Vector3();
            if (!Input.GetMouseButton(mouseButtonNumber)) return new Vector3();
            var hit = GetRaycastHit();
            if (hit.collider == null) return new Vector3();
            return hit.collider.gameObject.tag != "Ground" ? new Vector3() : hit.point;
        }

        private RaycastHit GetRaycastHit()
        {
            RaycastHit hit;
            var ray = _cameraController.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            return hit;
        }

        private readonly CameraController _cameraController;
    }
}
