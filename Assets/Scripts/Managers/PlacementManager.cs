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
            PlacedDominoManager placedDominoManager
            //PlacedObjectManager placedObjectManager
            )
        {
            _settings = settings;
            _prefabFactory = prefabFactory;
            _placedDominoManager = placedDominoManager;
            //_placedObjectManager = placedObjectManager;
            _typeDict = new Dictionary<Type, int>
            {
                { typeof(Domino), 0 },
                { typeof(Dominos), 1 }
            };
        }

        public void OnCreate(IPlacementable model)
        {
            DestroyGhost();
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
            if (_ghostObject == null) return;
            var ghostPos = Functions.GetPlacementPosition() == Vector3.zero ? Vector3.zero : Functions.GetPlacementPosition() + new Vector3(0, 0.6f, 0);
            if (ghostPos == Vector3.zero) return;
            if (!IsValidPosition(ghostPos, _ghostObject, _placedDominoManager)) return;
            UpdateGhostPosition(ghostPos, _ghostObject);
            if (Functions.GetMouseButtonInput(0)) AddItem(_ghostObject, _typeDict[_ghostPlaceObject.GetType()]);
        }

        private void UpdateGhostPosition(Vector3 ghostPos, GameObject ghostObject)
        {
            ghostObject.transform.position = ghostPos;
            ghostObject.transform.rotation = GetDefaultRotation();
        }

        private void AddItem(GameObject ghost, int typeDict)
        {
            switch (typeDict)
            {
                case 0:
                    PlaceDomino(ghost);
                    return;
                case 1:
                    PlaceMutliDomino(ghost);
                    return;
                //case 2:
                //    PlaceProp(ghostPos);
                //    break;
                default:
                    Debug.Log("PlacementManager.AddItem: Unknown Type");
                    return;
                
            }
        }

        private bool IsValidPosition(Vector3 pos, GameObject ghostObject, PlacedDominoManager dominoManager)
        {
            if (Functions.CanPlaceObject(dominoManager.GetPlacedDominos(), pos, 0.5f)) return true;
            ghostObject.transform.position = new Vector3(999, 999, 999);
            return false;
        }

        private void PlaceDomino(GameObject ghost)
        {
            _placedDominoManager.PlaceDomino(ghost.transform.position, GetSingleDominoRotation(ghost.transform.position));
            _timeLastPlaced = Time.time;
        }

        //private void PlaceProp(Vector3 ghostPos)
        //{
        //    if (_mouseLock) return;
        //    var dom = (Dominos)_ghostPlaceObject;
        //    if (dom == null) return;
        //    _placedObjectManager.AddObject(dom.gameObject, ghostPos, dom.gameObject.transform.rotation);
        //    foreach (var o in dom.Domino) _placedDominoManager.PlaceDomino(o.transform.position, o.transform.rotation);
        //    _mouseLock = true;
        //}

        private void PlaceMutliDomino(GameObject ghost)
        {
            if (_mouseLock) return;
            var dom = ghost.GetComponent<Dominos>();
            if (dom == null) return;
            foreach (var o in dom.Domino) _placedDominoManager.PlaceDomino(o.transform.position, o.transform.rotation);
            _mouseLock = true;
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

        private static Quaternion GetDefaultRotation()
        {
            var rotation = CameraManager.Instance.TheCamera.transform.rotation.eulerAngles;
            rotation = new Vector3(0, rotation.y, rotation.z);
            return Quaternion.Euler(rotation);
        }

        private bool _mouseLock;
        private float _timeLastPlaced;
        private Vector3 _positionLastPlaced;
        private GameObject _ghostObject;
        private IPlacementable _ghostPlaceObject;
        
        private readonly Settings _settings;
        private readonly PrefabFactory _prefabFactory;
        private readonly Dictionary<Type, int> _typeDict;
        //private readonly PlacedObjectManager _placedObjectManager;
        private readonly PlacedDominoManager _placedDominoManager;
        private readonly Dictionary<IPlacementable, GameObject> _spawnedGhostPlaceObjects = new Dictionary<IPlacementable, GameObject>();

        [Serializable]
        public class Settings
        {
            public float TimeToLine;
        }
    }
}
