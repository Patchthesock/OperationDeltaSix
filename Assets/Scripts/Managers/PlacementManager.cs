using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlacementManager : MonoBehaviour
    {
        public GameObject ObjectToPlace;

        [HideInInspector] public static PlacementManager instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (_gameManager == null)
            {
                _gameManager = GameManager.instance;
                return;
            }

            if (_placedObjectManager == null)
            {
                _placedObjectManager = PlacedObjectManager.instance;
                return;
            }

            var placementPosition = GetPlacementPosition(0);
            if (placementPosition == new Vector3() || !_placedObjectManager.CanPlaceObject(placementPosition) || !_canPlace) return;
            _canPlace = true;
            PlaceObject(ObjectToPlace, placementPosition, GetDefaultRotation());
        }

        private static Vector3 GetPlacementPosition(int mouseButtonNumber)
        {
            if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return new Vector3();
            if (!Input.GetMouseButton(mouseButtonNumber)) return new Vector3();
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            return hit.collider.gameObject.tag != "Ground" ? new Vector3() : hit.point;
        }

        public void PlaceObject(IEnumerable<SaveManager.ObjectPosition> positions)
        {
            PlacedObjectManager.instance.RemoveObjects();
            foreach (var p in positions)
            {
                PlaceObject(ObjectToPlace, p.Position, p.Rotation);
                Debug.Log("fired");
            }
        }

        private void PlaceObject(GameObject model, Vector3 position, Quaternion rotation)
        {
            _placedObjectManager.AddObject(model, position, rotation);
        }

        private static Quaternion GetDefaultRotation()
        {
            var rotation = CameraManager.instance.Camera.transform.rotation.eulerAngles;
            rotation = new Vector3(0, rotation.y, rotation.z);
            return Quaternion.Euler(rotation);
        }

        private static void RotateObject(GameObject objectToRoate)
        {
            
        }

        private bool _canPlace = true;
        private GameManager _gameManager;
        private PlacedObjectManager _placedObjectManager;
    }
}
