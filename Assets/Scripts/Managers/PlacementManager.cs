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

            var placementPosition = GetPlacementPosition(0);
            if (placementPosition != new Vector3())
            {
                PlaceObject(ObjectToPlace, placementPosition);
            }
        }

        private static Vector3 GetPlacementPosition(int mouseButtonNumber)
        {
            if (!Input.GetMouseButtonDown(mouseButtonNumber)) return new Vector3();
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return !Physics.Raycast(ray, out hit, Mathf.Infinity) ? new Vector3() : hit.normal;
        }

        private static void PlaceObject(GameObject model, Vector3 position)
        {
            var objectToPlace = Instantiate(model);
            objectToPlace.transform.position = position;
        }

        private GameManager _gameManager;
    }
}
