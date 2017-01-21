using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class PlacedObjectManager : MonoBehaviour
    {
        public float MinDistanceBetweenObjects;

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

        public void UpdatePlacedObjectPhysics(bool usePhysics)
        {
            foreach (var o in _placedObjects)
            {
                o.GetComponentInChildren<Rigidbody>().useGravity = usePhysics;
                o.GetComponentInChildren<Rigidbody>().isKinematic = !usePhysics;
            }
        }

        public void AddObject(GameObject model, Vector3 position, Quaternion rotation)
        {
            GameObject objectToPlace;
            if (_nonPlacedObjects.Count > 0)
            {
                objectToPlace = _nonPlacedObjects.First();
                _nonPlacedObjects.Remove(objectToPlace);
                objectToPlace.SetActive(true);
            }
            else
            {
                objectToPlace = Instantiate(model);
            }
            
            objectToPlace.GetComponentInChildren<Rigidbody>().isKinematic = true;
            objectToPlace.GetComponentInChildren<Rigidbody>().useGravity = false;
            objectToPlace.transform.position = position + new Vector3(0, 1f, 0);
            objectToPlace.transform.rotation = rotation;
            if (_placedObjects.Contains(objectToPlace)) return;
            _placedObjects.Add(objectToPlace);
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
            _nonPlacedObjects.Add(o);
            o.SetActive(false);
        }

        public void RemoveObjects()
        {
            foreach (var o in _placedObjects.ToList())
            {
                _placedObjects.Remove(o);
                _nonPlacedObjects.Add(o);
                o.SetActive(false);
            }
        }

        private readonly List<GameObject> _placedObjects = new List<GameObject>();
        private readonly List<GameObject> _nonPlacedObjects = new List<GameObject>();
    }
}
