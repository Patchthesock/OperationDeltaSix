using System.Collections.Generic;
using Assets.Scripts.Components;
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

        public float TimeToLine;
        public CursorMode normalCursor;
        public Texture2D cursorCantPlaceTexture;

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
            if (Input.GetMouseButtonUp(0))
            {
                _mouseLock = false;
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                DestroyGhost();
            }
            Setup();

            if (_removingObjects)
            {
                RemoveItem();
                return;
            }

            if (_selectedObject == null) return;
            var ghostPos = GetPlacementPosition();

            if (ghostPos == new Vector3())
            {
                _ghostObject.transform.position = new Vector3(999, 999, 999);
                Cursor.SetCursor(cursorCantPlaceTexture, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                _ghostObject.transform.position = ghostPos;
                Cursor.SetCursor(null, Vector2.zero, normalCursor);
            }

            _ghostObject.transform.rotation = GetDefaultRotation();
            var placementPosition = GetPlacementPosition(0);
            if (placementPosition == new Vector3()) return;
            
            // Give to Place Manager
            if (_selectedObject.tag == "MultiDomino")
            {
                if (_mouseLock) return;
                if (!_placedDominoManager.CanPlaceDomino(placementPosition)) return;
                var dom = _ghostObject.GetComponent<DominoHooks>();
                if (dom == null) return;

                foreach (var o in dom.Dominos)
                {
                    _placedDominoManager.PlaceDomino(
                        new Vector3(o.transform.position.x, o.transform.position.y - 1, o.transform.position.z),
                        o.transform.rotation);
                }
                ForceMouseButtonRelease();
            }
            if (_selectedObject.tag == "Domino")
            {
                if (!_placedDominoManager.CanPlaceDomino(placementPosition)) return;
                _placedDominoManager.PlaceDomino(placementPosition, GetSingleRotation(placementPosition));
                _timeLastPlaced = Time.time;
            }
            if (_selectedObject.tag == "Object") { }
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
                PlaceObject(_selectedObject, p.Position, p.Rotation);
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

        private Quaternion GetSingleRotation(Vector3 pos)
        {
            
            if (Time.time - _timeLastPlaced <= TimeToLine)
            {
                //return Quaternion.
            }
            _positionLastPlaced = pos;
            return GetDefaultRotation();
        }

        private void SetMenu(bool state)
        {
            DominoOneBtn.gameObject.SetActive(state);
            DominoTwoBtn.gameObject.SetActive(state);
            RemoveBtn.gameObject.SetActive(state);
        }

        private void SetObject(GameObject model)
        {
            // Ghost the Object
            _removingObjects = false;
            _selectedObject = model;
            _ghostObject = Instantiate(_selectedObject);
            
            if (_ghostObject.GetComponent<DominoHooks>())
            {
                foreach (var o in _ghostObject.GetComponent<DominoHooks>().Dominos)
                {
                    TurnOffDominoPhysics(o);
                }
            }
            else
            {
                TurnOffDominoPhysics(_ghostObject);
            }
        }

        private void RemoveObjects()
        {
            _removingObjects = true;
            _selectedObject = null;
        }

        private void DestroyGhost()
        {
            _selectedObject = null;
            if (_ghostObject == null) return;
            Destroy(_ghostObject);
        }

        private static void TurnOffDominoPhysics(GameObject o)
        {
            o.gameObject.layer = 2;
            if (o.GetComponent<Collider>() != null)
            {
                o.GetComponent<Collider>().isTrigger = true;
            }

            if (o.GetComponent<Rigidbody>() == null) return;
            o.GetComponent<Rigidbody>().isKinematic = true;
            o.GetComponent<Rigidbody>().useGravity = false;
        }

        private static void RemoveItem()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (hit.collider == null) return;

            switch (hit.collider.gameObject.tag)
            {
                case "Ground":
                    return;
                case "Domino":
                    PlacedDominoManager.instance.RemoveDomino(hit.collider.gameObject);
                    break;
                case "Object":
                    PlacedObjectManager.instance.RemoveObject(hit.collider.gameObject);
                    break;
            }
        }

        private void Setup()
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

            if (_placedDominoManager == null)
            {
                _placedDominoManager = PlacedDominoManager.instance;
            }
        }

        private void ForceMouseButtonRelease()
        {
            _mouseLock = true;
        }

        private bool _mouseLock = false;
        private bool _removingObjects = false;
        private bool _isActive = true;
        private Vector3 _positionLastPlaced;
        private GameObject _selectedObject;
        private GameObject _ghostObject;
        private GameManager _gameManager;
        private PlacedObjectManager _placedObjectManager;
        private PlacedDominoManager _placedDominoManager;
        private float _timeLastPlaced;


        [HideInInspector]
        public static PlacementManager instance = null;

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
                DestroyGhost();
                SetObject(SingleDomino);
            });

            DominoTwoBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                SetObject(FiveDomino);
            });

            RemoveBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                RemoveObjects();
            });
        }
    }
}
