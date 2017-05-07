using System;
using System.Collections.Generic;
using Assets.Scripts.Components;
using Assets.Scripts.Components.GameModels;
using Assets.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class PlacementManager : ITickable
    {
        public PlacementManager(
            Settings settings,
            PrefabFactory prefabFactory,
            PlacedDominoManager placedDominoManager,
            PlacedObjectManager placedObjectManager)
        {
            _settings = settings;
            _prefabFactory = prefabFactory;
            _placedDominoManager = placedDominoManager;
            _placedObjectManager = placedObjectManager;
            _typeDict = new Dictionary<Type, int>
            {
                { typeof(Domino), 0 },
                { typeof(Dominos), 1 }
            };
        }

        public void OnCreate(IPlacementable model)
        {
            DestroyGhost();
            _removingObjects = false;
            _ghostPlaceObject = model;
            _ghostObject = GetPlacementGameObject(model);
        }

        public void DestroyGhost()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            _ghostPlaceObject = null;
            if (_ghostObject == null) return;
            _ghostObject.transform.position = new Vector3(999, 999, 999);
            _ghostObject = null;
        }

        public void Tick()
        {
            if (Input.GetKey(KeyCode.Escape)) DestroyGhost();
            if (Input.GetMouseButtonUp(0) && _mouseLock) _mouseLock = false;
            if (_removingObjects)
            {
                RemoveItem();
                return;
            }


            // TODO: Problem here
            var ghostPos = GetPlacementPosition() + new Vector3(0, 0.6f, 0);
            UpdateGhostPosition(ghostPos, _ghostObject, _placedDominoManager);
            AddItem(ghostPos, _ghostObject, _placedDominoManager);
        }


        private void UpdateGhostPosition(Vector3 ghostPos, GameObject ghostObject, PlacedDominoManager dominoManager)
        {
            if (ghostObject == null) return;
            if (ghostPos == new Vector3() || !Functions.CanPlaceObject(dominoManager.GetPlacedDominos(), ghostPos, 0.5f))
            {
                ghostObject.transform.position = new Vector3(999, 999, 999);
                Cursor.SetCursor(_settings.CursorCantPlaceTexture, Vector2.zero, CursorMode.Auto);
                return;
            }
            ghostObject.transform.position = ghostPos;
            ghostObject.transform.rotation = GetDefaultRotation();
            Cursor.SetCursor(null, Vector2.zero, _settings.NormalCursor);
        }

        private void AddItem(Vector3 ghostPos, GameObject ghostObject, PlacedDominoManager dominoManager)
        {
            if (ghostObject == null) return;
            var placementPosition = GetPlacementPosition(0);
            if (placementPosition == new Vector3()) return;
            if (!Functions.CanPlaceObject(dominoManager.GetPlacedDominos(), placementPosition, 0.5f))
            {
                Cursor.SetCursor(_settings.CursorCantPlaceTexture, Vector2.zero, CursorMode.Auto);
                return;
            }

            switch (ghostObject.tag)
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

        private void PlaceDomino(Vector3 ghostPos)
        {
            _placedDominoManager.PlaceDomino(ghostPos, GetSingleDominoRotation(ghostPos));
            _timeLastPlaced = Time.time;
        }

        private void PlaceProp(Vector3 ghostPos)
        {
            if (_mouseLock) return;
            var dom = (Dominos)_ghostPlaceObject;
            if (dom == null) return;
            _placedObjectManager.AddObject(dom.gameObject, ghostPos, dom.gameObject.transform.rotation);
            foreach (var o in dom.Domino) _placedDominoManager.PlaceDomino(o.transform.position, o.transform.rotation);
            _mouseLock = true;
        }

        private void PlaceMutliDomino(Vector3 ghostPos)
        {
            if (_mouseLock) return;
            var dom = (Dominos)_ghostPlaceObject;
            if (dom == null) return;
            foreach (var o in dom.Domino) _placedDominoManager.PlaceDomino(new Vector3(o.transform.position.x, ghostPos.y, o.transform.position.z), o.transform.rotation);
            _mouseLock = true;
        }

        private static Vector3 GetPlacementPosition(int mouseButtonNumber)
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return new Vector3();
            return !Input.GetMouseButton(mouseButtonNumber) ? new Vector3() : GetPlacementPosition();
        }

        private static Vector3 GetPlacementPosition()
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (hit.collider == null) return new Vector3();
            return hit.collider.gameObject.tag != "Ground" ? new Vector3() : hit.point;
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
            return GetDefaultRotation();
        }

        private static Quaternion GetDefaultRotation()
        {
            var rotation = CameraManager.Instance.TheCamera.transform.rotation.eulerAngles;
            rotation = new Vector3(0, rotation.y, rotation.z);
            return Quaternion.Euler(rotation);
        }

        private GameObject GetPlacementGameObject(IPlacementable model)
        {
            if (_spawnedGhostPlaceObjects.ContainsKey(model)) return _spawnedGhostPlaceObjects[model];
            var ghostObject = _prefabFactory.Instantiate(model.GetGameObject()); // Ghost the Object
            _spawnedGhostPlaceObjects.Add(model, ghostObject);

            switch (_typeDict[model.GetType()])
            {
                case 0:
                    Functions.TurnOffGameObjectPhysics(ghostObject);
                    break;
                case 1:
                    var d = (Dominos)model;
                    foreach (var o in d.Domino) Functions.TurnOffGameObjectPhysics(o.gameObject);
                    break;
            }
            return ghostObject;
        }

        private void RemoveItem()
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
                    _placedDominoManager.RemoveDomino(hit.collider.gameObject);
                    return;
                case "Object":
                    _placedObjectManager.RemoveObject(hit.collider.gameObject);
                    return;
            }
        }

        private bool _mouseLock;
        private bool _removingObjects;
        private float _timeLastPlaced;
        private Vector3 _positionLastPlaced;
        private GameObject _ghostObject;
        private IPlacementable _ghostPlaceObject;
        
        private readonly Settings _settings;
        private readonly PrefabFactory _prefabFactory;
        private readonly Dictionary<Type, int> _typeDict;
        private readonly PlacedObjectManager _placedObjectManager;
        private readonly PlacedDominoManager _placedDominoManager;
        private readonly Dictionary<IPlacementable, GameObject> _spawnedGhostPlaceObjects = new Dictionary<IPlacementable, GameObject>();

        [Serializable]
        public class Settings
        {
            public float TimeToLine;
            public CursorMode NormalCursor;
            public Texture2D CursorCantPlaceTexture;
        }
    }
}
