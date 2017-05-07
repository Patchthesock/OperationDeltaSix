using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlacedObjectManager
    {
        public void AddObject(GameObject model, Vector3 position, Quaternion rotation)
        {
            var objectToPlace = Instantiate(model);
            foreach (Transform p in objectToPlace.transform) foreach (Transform d in p.transform) if (d.gameObject.tag == "Domino") Destroy(d.gameObject);
            objectToPlace.GetComponentInChildren<Rigidbody>().isKinematic = true;
            objectToPlace.GetComponentInChildren<Rigidbody>().useGravity = false;
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
            Destroy(o);
        }

        private readonly List<GameObject> _placedObjects = new List<GameObject>();
    }
}
