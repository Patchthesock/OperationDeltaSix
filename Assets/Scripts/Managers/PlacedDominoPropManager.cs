using System.Collections.Generic;
using Assets.Scripts.Components;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlacedDominoPropManager
    {
        public PlacedDominoPropManager(PrefabFactory prefabFactory)
        {
            _prefabFactory = prefabFactory;
        }

        public void AddObject(GameObject model, Vector3 position, Quaternion rotation)
        {
            var objectToPlace = _prefabFactory.Instantiate(model);
            objectToPlace.transform.position = position;
            objectToPlace.transform.rotation = rotation;
            if (_placedObjects.Contains(objectToPlace)) return;
            _placedObjects.Add(objectToPlace);
        }

        public void AddObject(IEnumerable<SaveManager.ObjectPosition> dominos)
        {
            foreach (var o in dominos) AddObject(o.GameObject, o.Position, o.Rotation);
        }

        public IEnumerable<GameObject> GetPlacedObjects()
        {
            return _placedObjects;
        } 

        public void RemoveObject(GameObject o)
        {
            _placedObjects.Remove(o);
            //Destroy(o);
        }

        private readonly PrefabFactory _prefabFactory;
        private readonly List<GameObject> _placedObjects = new List<GameObject>();
    }
}
