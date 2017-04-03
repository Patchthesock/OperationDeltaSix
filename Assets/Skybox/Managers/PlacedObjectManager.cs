using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class PlacedObjectManager : MonoBehaviour
    {
        public float MinDistanceBetweenObjects;

        public void AddObject(GameObject model, Vector3 position, Quaternion rotation)
        {
            var objectToPlace = Instantiate(model);
            objectToPlace.GetComponentInChildren<Rigidbody>().isKinematic = true;
            objectToPlace.GetComponentInChildren<Rigidbody>().useGravity = false;
            objectToPlace.transform.position = position + new Vector3(0, 1f, 0);
            objectToPlace.transform.rotation = rotation;
            if (_placedObjects.Contains(objectToPlace)) return;
            _placedObjects.Add(objectToPlace);
        }

        public void AddObject(IEnumerable<SaveManager.ObjectPosition> dominos)
        {
            foreach (var o in dominos)
            {
                AddObject(o.GameObject, o.Position, o.Rotation);
            }
        }

        public IEnumerable<GameObject> GetPlacedObjects()
        {
            return _placedObjects;
        } 

        public bool CanPlaceObject(Vector3 position)
        {
            return _placedObjects.All(o => !(Vector3.Distance(o.transform.position, position) < MinDistanceBetweenObjects));
        }

        public void RemoveObject(GameObject o)
        {
            _placedObjects.Remove(o);
            Destroy(o);
        }

        public void RemoveObjects()
        {
            foreach (var o in _placedObjects.ToList())
            {
                RemoveObject(o);
            }
        }

        private readonly List<GameObject> _placedObjects = new List<GameObject>();

        [HideInInspector]
        public static PlacedObjectManager instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }
    }
}
