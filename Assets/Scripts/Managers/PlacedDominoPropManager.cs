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

        public void AddObject(IEnumerable<SaveManager.SaveObject> model)
        {
            foreach (var o in model) AddObject(o.Name, o.Position, o.Rotation);
        }

        private void AddObject(string name, Vector3 position, Quaternion rotation)
        {
            var r = Resources.Load(name, typeof(GameObject)) as GameObject;
            if (r == null) return;
            AddObject(r, position, rotation);
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
