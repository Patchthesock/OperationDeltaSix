
using Assets.Scripts.Hooks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class PlacementManager : MonoBehaviour
    {
        // Domino Buttons
        public Button DominoOneBtn;
        public Button DominoFiveBtn;
        public Button DominoTenBtn;
        public Button DominoTwentyBtn;
        public Button DominoNintyLeftBtn;
        public Button DominoNintyRightBtn;
        public Button DominoOneEightyTurnBtn;

        // Prop Buttons
        public Button PropBridgeBtn;
        public Button PropStepSlideBtn;

        public Button ClearDominos;
        public Button RemoveBtn;
        public Button DominoMenuBtn;
        public Button PropMenuBtn;

        public GameObject SingleDomino;
        public GameObject FiveDomino;
        public GameObject TenDomino;
        public GameObject TwentyDomino;
        public GameObject NintyLeft;
        public GameObject NintyRight;
        public GameObject OneEightyTurn;

        public GameObject BridgeProp;
        public GameObject StepSlideProp;

        public GameObject MainUI;

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
            if (ghostPos == new Vector3() || !_placedDominoManager.CanPlaceDomino(ghostPos))
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
                if (!_placedDominoManager.CanPlaceDomino(placementPosition))
                {
                    Cursor.SetCursor(cursorCantPlaceTexture, Vector2.zero, CursorMode.Auto);
                }
                if (!_placedDominoManager.CanPlaceDomino(placementPosition)) return;
                var dom = _ghostObject.GetComponent<DominoHooks>();
                if (dom == null) return;

                //foreach (var o in dom.Dominos)
                //{
                //    _placedDominoManager.PlaceDomino(
                //        new Vector3(o.transform.position.x, ghostPos.y, o.transform.position.z),
                //        o.transform.rotation);
                //}
                ForceMouseButtonRelease();
            }
            if (_selectedObject.tag == "Domino")
            {
                if (!_placedDominoManager.CanPlaceDomino(placementPosition))
                {
                    Cursor.SetCursor(cursorCantPlaceTexture, Vector2.zero, CursorMode.Auto);
                }
                if (!_placedDominoManager.CanPlaceDomino(ghostPos)) return;
                _placedDominoManager.PlaceDomino(ghostPos, GetSingleRotation(ghostPos));
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
            return hit.collider.gameObject.tag != "Ground" ? new Vector3() : hit.point + new Vector3(0,0.6f,0);
        }

        private static Quaternion GetDefaultRotation()
        {
            var rotation = CameraManager.instance.TheCamera.transform.rotation.eulerAngles;
            rotation = new Vector3(0, rotation.y, rotation.z);
            return Quaternion.Euler(rotation);
        }

        private Quaternion GetSingleRotation(Vector3 pos)
        {
            if (Time.time - _timeLastPlaced <= TimeToLine)
            {
                var rot = Quaternion.LookRotation(_positionLastPlaced - pos);
                _positionLastPlaced = pos;
                return rot;
            }
            _positionLastPlaced = pos;
            return GetDefaultRotation();
        }

        private void SetMenu(bool state)
        {
            DominoMenuBtn.gameObject.SetActive(state);
            PropMenuBtn.gameObject.SetActive(state);
        }

        private void SetObject(GameObject model)
        {
            // Ghost the Object
            _removingObjects = false;
            _selectedObject = model;
            _ghostObject = Instantiate(_selectedObject);
            
            if (_ghostObject.GetComponent<DominoHooks>())
            {
                //foreach (var o in _ghostObject.GetComponent<DominoHooks>().Dominos)
                //{
                //    TurnOffDominoPhysics(o);
                //}
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
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
				foreach(Transform child in MainUI.transform)
				{
					child.GetComponent<Button>().interactable = true;
					 GameObject.Find("UIControl").GetComponent<UIManager>().InventoryOpen = false;
					 GameObject.Find("UIControl").GetComponent<UIManager>().ActiveInventory.SetActive(false);
				}
            });

            DominoFiveBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                SetObject(FiveDomino);
				foreach(Transform child in MainUI.transform)
				{
					child.GetComponent<Button>().interactable = true;
					GameObject.Find("UIControl").GetComponent<UIManager>().InventoryOpen = false;
					GameObject.Find("UIControl").GetComponent<UIManager>().ActiveInventory.SetActive(false);
				}
            });

            DominoTenBtn.onClick.AddListener(() =>
            {
				foreach(Transform child in MainUI.transform)
				{
					child.GetComponent<Button>().interactable = true;
					GameObject.Find("UIControl").GetComponent<UIManager>().InventoryOpen = false;
					GameObject.Find("UIControl").GetComponent<UIManager>().ActiveInventory.SetActive(false);
				}
                DestroyGhost();
                SetObject(TenDomino);
            });

            DominoTwentyBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                SetObject(TwentyDomino);
				foreach(Transform child in MainUI.transform)
				{
					child.GetComponent<Button>().interactable = true;
					GameObject.Find("UIControl").GetComponent<UIManager>().InventoryOpen = false;
					GameObject.Find("UIControl").GetComponent<UIManager>().ActiveInventory.SetActive(false);
				}				
            });

            DominoNintyLeftBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                SetObject(NintyLeft);
				foreach(Transform child in MainUI.transform)
				{
					child.GetComponent<Button>().interactable = true;
					GameObject.Find("UIControl").GetComponent<UIManager>().InventoryOpen = false;
					GameObject.Find("UIControl").GetComponent<UIManager>().ActiveInventory.SetActive(false);
				}				
            });

            DominoNintyRightBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                SetObject(NintyRight);
				foreach(Transform child in MainUI.transform)
				{
					child.GetComponent<Button>().interactable = true;
					GameObject.Find("UIControl").GetComponent<UIManager>().InventoryOpen = false;
					GameObject.Find("UIControl").GetComponent<UIManager>().ActiveInventory.SetActive(false);
				}
            });

            DominoOneEightyTurnBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                SetObject(OneEightyTurn);
				foreach(Transform child in MainUI.transform)
				{
					child.GetComponent<Button>().interactable = true;
					GameObject.Find("UIControl").GetComponent<UIManager>().InventoryOpen = false;
					GameObject.Find("UIControl").GetComponent<UIManager>().ActiveInventory.SetActive(false);
				}				
            });

            PropBridgeBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                SetObject(BridgeProp);
                foreach (Transform child in MainUI.transform)
                {
                    child.GetComponent<Button>().interactable = true;
                    GameObject.Find("UIControl").GetComponent<UIManager>().InventoryOpen = false;
                    GameObject.Find("UIControl").GetComponent<UIManager>().ActiveInventory.SetActive(false);
                }
            });

            PropStepSlideBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                SetObject(StepSlideProp);
                foreach (Transform child in MainUI.transform)
                {
                    child.GetComponent<Button>().interactable = true;
                    GameObject.Find("UIControl").GetComponent<UIManager>().InventoryOpen = false;
                    GameObject.Find("UIControl").GetComponent<UIManager>().ActiveInventory.SetActive(false);
                }
            });

            ClearDominos.onClick.AddListener(() =>
            {
                PlacedDominoManager.instance.RemoveDomino();
                DestroyGhost();
                foreach (Transform child in MainUI.transform)
                {
                    child.GetComponent<Button>().interactable = true;
                    GameObject.Find("UIControl").GetComponent<UIManager>().InventoryOpen = false;
                    GameObject.Find("UIControl").GetComponent<UIManager>().ActiveInventory.SetActive(false);
                }
            });

            RemoveBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                RemoveObjects();
            });
        }
    }
}
