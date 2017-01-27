using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class PlacementController
    {
        public PlacementController(
            Settings settings,
            CameraController cameraController)
        {
            _settings = settings;
            _cameraController = cameraController;
        } 

        private Quaternion GetSingleDominoRotation(Vector3 pos)
        {
            if (Time.time - _timeLastPlaced <= _settings.TimeToLine)
            {
                var rot = Quaternion.LookRotation(_positionLastPlaced - pos);
                _positionLastPlaced = pos;
                return rot;
            }
            _positionLastPlaced = pos;
            _timeLastPlaced = Time.time;
            return GetDefaultRotation();
        }

        private Vector3 GetPointerPosition()
        {
            var hit = GetRaycastHit();
            if (hit.collider == null) return new Vector3();
            return hit.collider.gameObject.tag != "Ground" ? new Vector3() : hit.point + new Vector3(0, 0.6f, 0);
        }

        private Vector3 GetClickPosition(int mouseButtonNumber)
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

        private static Quaternion GetDefaultRotation()
        {
            var rotation = CameraManager.instance.TheCamera.transform.rotation.eulerAngles;
            rotation = new Vector3(0, rotation.y, rotation.z);
            return Quaternion.Euler(rotation);
        }

        private float _timeLastPlaced;
        private Vector3 _positionLastPlaced;
        private readonly Settings _settings;
        private readonly CameraController _cameraController;

        public class Settings
        {
            public float TimeToLine;
        }
    }
}
