using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components;
using Assets.Scripts.Managers.Models;
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
            var objectToPlace = TryGetExistingObject(model.name) ?? GetNewObject(model);
            objectToPlace.transform.position = position;
            objectToPlace.transform.rotation = rotation;
            _activeObjects.Add(objectToPlace);
        }

        public void AddObject(IEnumerable<SaveObject> model)
        {
            RemoveObject();
            foreach (var o in model) AddObject(o.Name, o.Position, o.Rotation);
        }

        private void AddObject(string name, Vector3 position, Quaternion rotation)
        {
            var r = Resources.Load("Props/"+name, typeof(GameObject)) as GameObject;
            if (r == null) return;
            AddObject(r, position, rotation);
        }

        public IEnumerable<GameObject> GetPlacedObjects()
        {
            return _activeObjects;
        } 

        public void RemoveObject(GameObject o)
        {
            Debug.Log(o);
            _activeObjects.Remove(o);
            _nonActiveObjects.Add(o);
            o.SetActive(false);
        }

        private void RemoveObject()
        {
            foreach (var m in _activeObjects.ToList()) RemoveObject(m);
        }

        private GameObject GetNewObject(GameObject model)
        {
            var m = _prefabFactory.Instantiate(model);
            m.name = model.name;
            return m;
        }

        private GameObject TryGetExistingObject(string name)
        {
            if (_nonActiveObjects.Count <= 0) return null;
            var m = _nonActiveObjects.First(i => i.name == name).gameObject;
            if (m != null) m.SetActive(true);
            return m;
        }

        private readonly PrefabFactory _prefabFactory;
        private readonly List<GameObject> _activeObjects = new List<GameObject>();
        private readonly List<GameObject> _nonActiveObjects = new List<GameObject>();
    }
}
