using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlacedDominoManager
    {
        public PlacedDominoManager(Settings settings)
        {
            _settings = settings;
        }

        public void PlaceDomino(Vector3 position, Quaternion rotation)
        {
            if (_nonActiveDominos.Count > 0)
            {
                var objectToPlace = _nonActiveDominos.First();
                _nonActiveDominos.Remove(objectToPlace);
                objectToPlace.SetActive(true);
                PlaceObject(objectToPlace, position,rotation);
            }
            else PlaceObject(Instantiate(Functions.PickRandomObject(_settings.Dominos)), position, rotation);
        }

        public void PlaceDomino(IEnumerable<SaveManager.ObjectPosition> dominos)
        {
            RemoveDomino();
            foreach (var o in dominos) PlaceDomino(o.Position, o.Rotation);
        }

        public void SetDominoPhysics(bool usePhysics)
        {
            foreach (var o in _placedDominos)
            {
                o.GetComponentInChildren<Rigidbody>().useGravity = usePhysics;
                o.GetComponentInChildren<Rigidbody>().isKinematic = !usePhysics;
            }
        }

        public IEnumerable<GameObject> GetPlacedDominos()
        {
            return _placedDominos;
        }

        public void RemoveDomino()
        {
            foreach (var o in _placedDominos.ToList()) RemoveDomino(o);
        }

        public void RemoveDomino(GameObject o)
        {
            _placedDominos.Remove(o);
            _nonActiveDominos.Add(o);
            o.SetActive(false);
        }

        private void PlaceObject(GameObject model, Vector3 position, Quaternion rotation)
        {
            model.GetComponentInChildren<Rigidbody>().isKinematic = true;
            model.GetComponentInChildren<Rigidbody>().useGravity = false;
            model.transform.position = position;
            model.transform.rotation = rotation;
            model.transform.SetParent(_settings.ARBoard.transform);
            if (_placedDominos.Contains(model)) return;
            _placedDominos.Add(model);
        }

        private readonly Settings _settings;
        private readonly List<GameObject> _placedDominos = new List<GameObject>();
        private readonly List<GameObject> _nonActiveDominos = new List<GameObject>();

        [Serializable]
        public class Settings
        {
            public GameObject ARBoard;
            public List<GameObject> Dominos;
        }
    }
}
