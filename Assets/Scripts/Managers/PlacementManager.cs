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
            CameraManager cameraManager,
            PlacedDominoManager placedDominoManager,
            PlacedDominoPropManager placedDominoPropManager)
        {
            _settings = settings;
            _prefabFactory = prefabFactory;
            _cameraManager = cameraManager;
            _placedDominoManager = placedDominoManager;
            _placedDominoPropManager = placedDominoPropManager;
            _typeDict = Functions.GetPlaceableTypeDictionary();
        }

        public void OnCreate(IPlacementable model)
        {
            DestroyGhost();
            _ghostPlaceObject = model;
            _ghostObject = GetPlacementGameObject(model);
        }

        public void DestroyGhost()
        {
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

        private void AddItem(GameObject ghost, int typeDict)
        {
            if (_settings.AudioSource == null) return;
            _settings.AudioSource.clip = _settings.PlacementClips[UnityEngine.Random.Range(0, _settings.PlacementClips.Count - 1)];
            if (!_settings.AudioSource.isPlaying) _settings.AudioSource.Play();

            switch (typeDict)
            {
                case 0:
                    PlaceDomino(ghost);
                    return;
                case 1:
                    PlaceDominos(ghost);
                    return;
                case 2:
                    PlaceDominosProp(ghost);
                    break;
                default:
                    Debug.Log("PlacementManager.AddItem: Unknown Type");
                    return;
            }
        }

        private void PlaceDomino(GameObject ghost)
        {
            _placedDominoManager.PlaceDomino(ghost.transform.position, GetSingleDominoRotation(ghost.transform.position));
            _timeLastPlaced = Time.time;
        }
        
        private void PlaceDominos(GameObject ghost)
        {
            if (_mouseLock) return;
            PlaceDominos(ghost.GetComponent<Dominos>());
            _mouseLock = true;
        }

        private void PlaceDominosProp(GameObject ghost)
        {
            if (_mouseLock) return;
            var dom = ghost.GetComponent<DominosProp>();
            PlaceDominos(dom.Dominos);
            if (dom.Prop == null) return;
            _placedDominoPropManager.AddObject(dom.Prop, ghost.transform.position, ghost.transform.rotation);
            _mouseLock = true;
        }

        private void PlaceDominos(Dominos dominos)
        {
            if (dominos == null) return;
            foreach (var o in dominos.Domino) _placedDominoManager.PlaceDomino(o.transform.position, o.transform.rotation);
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
            return GetDefaultRotation(_cameraManager.GetCameraRotation());
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
                    var d = (Dominos) model;
                    foreach (var o in d.Domino) Functions.TurnOffGameObjectPhysics(o.gameObject);
                    break;
                case 2:
                    var e = (DominosProp) model;
                    //Functions.TurnOffGameObjectPhysics(e.GetGameObject());
                    Functions.TurnOffGameObjectPhysics(e.GhostProp);
                    foreach (var o in e.Dominos.Domino) Functions.TurnOffGameObjectPhysics(o.gameObject);
                    break;
                default:
                    Debug.Log("PlacementManager.GetPlacementGameObject(IPlacementable): Unknown type");
                    break;
            }
            return ghostObject;
        }

        private void UpdateGhostPosition(Vector3 ghostPos, GameObject ghostObject)
        {
            ghostObject.transform.position = ghostPos;
            ghostObject.transform.rotation = GetDefaultRotation(_cameraManager.GetCameraRotation());
        }

        private static Quaternion GetDefaultRotation(Quaternion cameraRotation)
        {
            var rotation = cameraRotation.eulerAngles;
            rotation = new Vector3(0, rotation.y, rotation.z);
            return Quaternion.Euler(rotation);
        }

        private static bool IsValidPosition(Vector3 pos, GameObject ghostObject, PlacedDominoManager dominoManager)
        {
            if (Functions.CanPlaceObject(dominoManager.GetPlacedDominos(), pos, 0.5f)) return true;
            ghostObject.transform.position = new Vector3(999, 999, 999);
            return false;
        }

        private bool _mouseLock;
        private float _timeLastPlaced;
        private Vector3 _positionLastPlaced;
        private GameObject _ghostObject;
        private IPlacementable _ghostPlaceObject;
        
        private readonly Settings _settings;
        private readonly PrefabFactory _prefabFactory;
        private readonly CameraManager _cameraManager;
        private readonly Dictionary<Type, int> _typeDict;
        private readonly PlacedDominoManager _placedDominoManager;
        private readonly PlacedDominoPropManager _placedDominoPropManager;
        private readonly Dictionary<IPlacementable, GameObject> _spawnedGhostPlaceObjects = new Dictionary<IPlacementable, GameObject>();

        [Serializable]
        public class Settings
        {
            public float TimeToLine;
            public AudioSource AudioSource;
            public List<AudioClip> PlacementClips;
        }
    }
}
