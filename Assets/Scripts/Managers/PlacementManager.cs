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
            if (placementPosition != new Vector3() && _placedObjectManager.CanPlaceObject(placementPosition) && _canPlace)
            {
                _canPlace = true;
                _placedObjectManager.AddObject(PlaceObject(ObjectToPlace, placementPosition));
            }
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

        private static GameObject PlaceObject(GameObject model, Vector3 position)
        {
            var objectToPlace = Instantiate(model);
            objectToPlace.transform.position = position;
            return objectToPlace;
        }

        private static void RotateObject(GameObject objectToRoate)
        {
            
        }

        private bool _canPlace = true;
        private GameManager _gameManager;
        private PlacedObjectManager _placedObjectManager;
    }
}
