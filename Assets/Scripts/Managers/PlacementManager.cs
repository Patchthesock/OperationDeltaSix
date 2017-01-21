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
            PlaceObject(ObjectToPlace, placementPosition);
        }

        private static Vector3 GetPlacementPosition(int mouseButtonNumber)
        {
            if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return new Vector3();
            if (!Input.GetMouseButtonDown(mouseButtonNumber)) return new Vector3();
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
                PlaceObject(ObjectToPlace, p.Position);
                Debug.Log("fired");
            }
        }

        private void PlaceObject(GameObject model, Vector3 position)
        {
            var objectToPlace = Instantiate(model);
            objectToPlace.GetComponentInChildren<Rigidbody>().isKinematic = true;
            objectToPlace.GetComponentInChildren<Rigidbody>().useGravity = false;
            objectToPlace.transform.position = position + new Vector3(0,1f,0);
            _placedObjectManager.AddObject(objectToPlace);
        }

        private static void RotateObject(GameObject objectToRoate)
        {
            
        }

        private bool _canPlace = true;
        private GameManager _gameManager;
        private PlacedObjectManager _placedObjectManager;
    }
}
