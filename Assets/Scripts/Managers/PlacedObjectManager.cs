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

        public void AddObject(GameObject placedObject)
        {
            if (_placedObjects.Contains(placedObject)) return;
            _placedObjects.Add(placedObject);
        }

        public IEnumerable<GameObject> GetPlacedObjects()
        {
            return _placedObjects;
        } 

        public bool CanPlaceObject(Vector3 position)
        {
            return _placedObjects.All(o => !(Vector3.Distance(o.transform.position, position) < MinDistanceBetweenObjects));
        }

        public void RemoveObjects()
        {
            foreach (var o in _placedObjects.ToList())
            {
                _placedObjects.Remove(o);
                Destroy(o);
            }
        }

        private readonly List<GameObject> _placedObjects = new List<GameObject>();
    }
}
