using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Factories;
using Assets.Scripts.Models;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Controllers
{
    public class GhostController : ITickable
    {
        public GhostController(
            Settings settings,
            PrefabFactory prefabFactory,
            InputController inputController,
            CameraController cameraController)
        {
            _settings = settings;
            _prefabFactory = prefabFactory;
            _inputController = inputController;
            _cameraController = cameraController;
        }

        public void Select(GameObject model)
        {
            Drop();
            _ghost = GetGhost(model);
        }

        public void Drop()
        {
            if (_ghost == null) return;
            _ghost.transform.position = new Vector3(999, 999, 999);
            _ghost = null;
        }

        public void Tick()
        {
            if (_ghost == null) return;
            _ghost.transform.position = _inputController.GetMousePosition();
            _ghost.transform.rotation = Quaternion.Lerp(_ghost.transform.rotation, GetSingleDominoRotation(_ghost.transform.position), Time.deltaTime * 50);
            if (_inputController.GetMouseClickPosition(0) != Vector3.zero)
                OnItemPlaced(ModelFactory.CreateObjectPlacementModel(
                    _ghost,
                    _ghost.transform.position,
                    _ghost.transform.rotation.eulerAngles));
        }

        public void SubscribeToOnItemPlaced(Action<ObjectPlacementModel> onItemPlaced)
        {
            if (_onItemPlacedListerns.Contains(onItemPlaced)) return;
            _onItemPlacedListerns.Add(onItemPlaced);
        }

        private void OnItemPlaced(ObjectPlacementModel model)
        {
            _timeLastPlaced = Time.time;
            _positionLastPlaced = model.Position;
            foreach (var l in _onItemPlacedListerns)
            {
                l(model);
            }
        }

        private GameObject GetGhost(GameObject model)
        {
            if (_cachedGhosts.ContainsKey(model.name)) return _cachedGhosts.FirstOrDefault(t => t.Key == model.name).Value;
            var ghost = _prefabFactory.Create(model);
            ghost.layer = 2;
            ghost.GetComponent<Rigidbody>().isKinematic = true;
            ghost.GetComponent<Rigidbody>().useGravity = false;
            _cachedGhosts.Add(model.name, ghost);
            return ghost;
        }

        private Quaternion GetSingleDominoRotation(Vector3 pos)
        {
            //if (Mathf.Abs(Time.time - _timeLastPlaced) > _settings.TimeToLine)
            //    return GetDefaultSingleDominoRotation();

            return Quaternion.LookRotation(_positionLastPlaced - pos)
                .eulerAngles == Vector3.zero ? GetDefaultSingleDominoRotation()
                : Quaternion.LookRotation(_positionLastPlaced - pos);
        }

        private Quaternion GetDefaultSingleDominoRotation()
        {
            var rotation = _cameraController.Rotation.eulerAngles;
            rotation = new Vector3(0, rotation.y, rotation.z);
            return Quaternion.Euler(rotation);
        }

        private GameObject _ghost;
        private float _timeLastPlaced;
        private Vector3 _positionLastPlaced;
        private readonly Settings _settings;
        private readonly PrefabFactory _prefabFactory;
        private readonly InputController _inputController;
        private readonly CameraController _cameraController;
        private readonly Dictionary<string, GameObject> _cachedGhosts = new Dictionary<string, GameObject>();
        private readonly List<Action<ObjectPlacementModel>> _onItemPlacedListerns = new List<Action<ObjectPlacementModel>>();

        [Serializable]
        public class Settings
        {
            public float TimeToLine;
            
        }
    }
}
