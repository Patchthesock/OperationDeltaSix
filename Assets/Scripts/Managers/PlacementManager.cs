using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class PlacementManager : MonoBehaviour
    {
        public Button DominoOneBtn;
        public Button DominoTwoBtn;
        public Button RemoveBtn;

        public GameObject SingleDomino;
        public GameObject FiveDomino;
        private GameObject ObjectToPlace;

        [HideInInspector] public static PlacementManager instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            _removingObjects = false;

            DominoOneBtn.onClick.AddListener(() =>
            {
                ObjectToPlace = SingleDomino;
                SetObject(ObjectToPlace);
            });

            DominoTwoBtn.onClick.AddListener(() =>
            {
                ObjectToPlace = FiveDomino;
                SetObject(ObjectToPlace);
            });

            RemoveBtn.onClick.AddListener(RemoveObjects);
        }

        public void SetActive(bool state)
        {
            _isActive = state;
            SetMenu(state);
            _removingObjects = false;
            _selectedObject = null;
            Destroy(_ghostObject);
        }

        private void Update()
        {
            if (!_isActive) return;
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

            if (_removingObjects)
            {
                if (!Input.GetMouseButtonDown(0)) return;
                RaycastHit hit;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                if (hit.collider == null) return;
                if (hit.collider.gameObject.tag != "Disposable") return;
                PlacedObjectManager.instance.RemoveObject(hit.collider.gameObject);
                return;
            }

            if (_selectedObject == null) return;
            var ghostPos = GetPlacementPosition();
            _ghostObject.transform.position = ghostPos == new Vector3() ? new Vector3(999, 999, 999) : ghostPos;
            _ghostObject.transform.rotation = GetDefaultRotation();
            var placementPosition = GetPlacementPosition(0);
            if (placementPosition == new Vector3() || !_placedObjectManager.CanPlaceObject(placementPosition)) return;

            if (ObjectToPlace.GetComponent<DominoHooks>())
            {
                foreach (var o in ObjectToPlace.GetComponent<DominoHooks>().Dominos)
                {
                    _placedObjectManager.AddObject(o, o.transform.position, GetDefaultRotation());
                }
            }
            else
            {
                PlaceObject(ObjectToPlace, placementPosition, GetDefaultRotation());
            }
        }

        private static Vector3 GetPlacementPosition(int mouseButtonNumber)
        {
            if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return new Vector3();
            if (!Input.GetMouseButton(mouseButtonNumber)) return new Vector3();
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (hit.collider == null) return new Vector3();
            return hit.collider.gameObject.tag != "Ground" ? new Vector3() : hit.point;
        }

        private static Vector3 GetPlacementPosition()
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (hit.collider == null) return new Vector3();
            return hit.collider.gameObject.tag != "Ground" ? new Vector3() : hit.point + new Vector3(0,1.1f,0);
        }

        public void PlaceObject(IEnumerable<SaveManager.ObjectPosition> positions)
        {
            PlacedObjectManager.instance.RemoveObjects();
            foreach (var p in positions)
            {
                PlaceObject(ObjectToPlace, p.Position, p.Rotation);
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

        private void SetMenu(bool state)
        {
            DominoOneBtn.gameObject.SetActive(state);
            DominoTwoBtn.gameObject.SetActive(state);
            RemoveBtn.gameObject.SetActive(state);
        }

        private void SetObject(GameObject model)
        {
            _removingObjects = false;
            _selectedObject = model;
            _ghostObject = Instantiate(_selectedObject);
            

            if (_ghostObject.GetComponent<DominoHooks>())
            {
                foreach (var o in _ghostObject.GetComponent<DominoHooks>().Dominos)
                {
                    o.gameObject.layer = 2;
                    if (o.GetComponent<Collider>() != null)
                    {
                        o.GetComponent<Collider>().isTrigger = true;
                    }

                    if (o.GetComponent<Rigidbody>() != null)
                    {
                        o.GetComponent<Rigidbody>().isKinematic = true;
                        o.GetComponent<Rigidbody>().useGravity = false;
                    }
                }
            }
            else
            {
                _ghostObject.gameObject.layer = 2;
                if (_ghostObject.GetComponent<Collider>() != null)
                {
                    _ghostObject.GetComponent<Collider>().isTrigger = true;
                }

                if (_ghostObject.GetComponent<Rigidbody>() != null)
                {
                    _ghostObject.GetComponent<Rigidbody>().isKinematic = true;
                    _ghostObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }

        private void RemoveObjects()
        {
            _removingObjects = true;
            _selectedObject = null;
            if (_ghostObject == null) return;
            Destroy(_ghostObject);
        }

        private bool _removingObjects = false;
        private bool _isActive = true;
        private GameObject _selectedObject;
        private GameObject _ghostObject;
        private GameManager _gameManager;
        private PlacedObjectManager _placedObjectManager;
    }
}
