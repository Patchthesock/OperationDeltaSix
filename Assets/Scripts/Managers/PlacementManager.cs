using Assets.Scripts.Components;
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

        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            if (!_isActive) return;
            if (Input.GetMouseButtonUp(0)) _mouseLock = false;
            if (Input.GetKey(KeyCode.Escape)) DestroyGhost();
            
            Setup();

            if (_removingObjects)
            {
                RemoveItem();
                return;
            }
            if (_selectedObject != null) AddItem(_selectedObject.tag, GetPlacementPosition(), _ghostObject, _placedDominoManager);
        }

        private void AddItem(string objectTag, Vector3 ghostPos, GameObject ghostObject, PlacedDominoManager dominoManager)
        {
            if (ghostPos == new Vector3() || !Functions.CanPlaceObject(dominoManager.GetPlacedDominos(), ghostPos, 0.5f))
            {
                ghostObject.transform.position = new Vector3(999, 999, 999);
                Cursor.SetCursor(cursorCantPlaceTexture, Vector2.zero, CursorMode.Auto);
                return;
            }
            ghostObject.transform.position = ghostPos;
            Cursor.SetCursor(null, Vector2.zero, normalCursor);

            ghostObject.transform.rotation = GetDefaultRotation();
            var placementPosition = GetPlacementPosition(0);
            if (placementPosition == new Vector3()) return;
            if (!Functions.CanPlaceObject(dominoManager.GetPlacedDominos(), placementPosition, 0.5f))
            {
                Cursor.SetCursor(cursorCantPlaceTexture, Vector2.zero, CursorMode.Auto);
                return;
            }

            switch (objectTag)
            {
                case "MultiDomino":
                    PlaceMutliDomino(ghostPos);
                    break;
                case "Domino":
                    PlaceDomino(ghostPos);
                    break;
                case "Object":
                    PlaceProp(ghostPos);
                    break;
            }
        }

        private void PlaceMutliDomino(Vector3 ghostPos)
        {
            if (_mouseLock) return;
            var dom = _ghostObject.GetComponent<DominoHooks>();
            if (dom == null) return;
            foreach (var o in dom.Dominos) _placedDominoManager.PlaceDomino(new Vector3(o.transform.position.x, ghostPos.y, o.transform.position.z), o.transform.rotation);
            _mouseLock = true;
        }

        private void PlaceDomino(Vector3 ghostPos)
        {
            _placedDominoManager.PlaceDomino(ghostPos, GetSingleRotation(ghostPos));
            _timeLastPlaced = Time.time;
        }

        private void PlaceProp(Vector3 ghostPos)
        {
            if (_mouseLock) return;
            _placedObjectManager.AddObject(_ghostObject, ghostPos, _ghostObject.transform.rotation);
            var dom = _ghostObject.GetComponent<DominoHooks>();
            if (dom == null) return;
            foreach (var o in dom.Dominos) _placedDominoManager.PlaceDomino(o.transform.position, o.transform.rotation);
            _mouseLock = true;
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
            _removingObjects = false;
            _selectedObject = model;
            _ghostObject = Instantiate(_selectedObject); // Ghost the Object
            if (_ghostObject.GetComponent<DominoHooks>()) foreach (var o in _ghostObject.GetComponent<DominoHooks>().Dominos) Functions.TurnOffGameObjectPhysics(o);
            else Functions.TurnOffGameObjectPhysics(_ghostObject);
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
                    PlacedDominoManager.Instance.RemoveDomino(hit.collider.gameObject);
                    break;
                case "Object":
                    PlacedObjectManager.Instance.RemoveObject(hit.collider.gameObject);
                    break;
            }
        }

        private void Setup()
        {
            if (_gameManager == null) _gameManager = GameManager.instance;
            if (_placedObjectManager == null) _placedObjectManager = PlacedObjectManager.Instance;
            if (_placedDominoManager == null) _placedDominoManager = PlacedDominoManager.Instance;
        }

        private void OnCreateButtonClick(GameObject model)
        {
            DestroyGhost();
            TurnOffMenuUi();
            SetObject(model);
        }

        private void TurnOffMenuUi()
        {
            _radialManager.InventoryOpen = false;
            _radialManager.MainUI.SetActive(false);
            _radialManager.ActiveInventory.SetActive(false);
        }

        private void SetupButtons()
        {
            DominoTenBtn.onClick.AddListener(() => { OnCreateButtonClick(TenDomino); });
            DominoFiveBtn.onClick.AddListener(() => { OnCreateButtonClick(FiveDomino); });
            PropBridgeBtn.onClick.AddListener(() => { OnCreateButtonClick(BridgeProp); });
            DominoOneBtn.onClick.AddListener(() => { OnCreateButtonClick(SingleDomino); });
            DominoNintyLeftBtn.onClick.AddListener(() => { OnCreateButtonClick(NintyLeft); });
            DominoTwentyBtn.onClick.AddListener(() => { OnCreateButtonClick(TwentyDomino); });
            PropStepSlideBtn.onClick.AddListener(() => { OnCreateButtonClick(StepSlideProp); });
            DominoNintyRightBtn.onClick.AddListener(() => { OnCreateButtonClick(NintyRight); });
            DominoOneEightyTurnBtn.onClick.AddListener(() => { OnCreateButtonClick(OneEightyTurn); });

            ClearDominos.onClick.AddListener(() =>
            {
                DestroyGhost();
                TurnOffMenuUi();
                PlacedDominoManager.Instance.RemoveDomino();
            });

            RemoveBtn.onClick.AddListener(() =>
            {
                DestroyGhost();
                RemoveObjects();
            });
        }

        private bool _mouseLock;
        private bool _removingObjects;
        private bool _isActive = true;
        private Vector3 _positionLastPlaced;
        private GameObject _selectedObject;
        private GameObject _ghostObject;
        private GameManager _gameManager;
        private RadialManager _radialManager;
        private PlacedObjectManager _placedObjectManager;
        private PlacedDominoManager _placedDominoManager;
        private float _timeLastPlaced;

        [HideInInspector]
        public static PlacementManager Instance;

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            // Setting up as Singleton
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            _removingObjects = false;

            // Setup Menu
            _radialManager = GameObject.Find("UIControl").GetComponent<RadialManager>();

            // Set up Buttons
            SetupButtons();
        }
    }
}
