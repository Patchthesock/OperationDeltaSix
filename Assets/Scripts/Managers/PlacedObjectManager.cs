using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class PlacedObjectManager : MonoBehaviour
    {
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

        public void AddObject(GameObject placedObject)
        {
            if (_placedObjects.Contains(placedObject)) return;
            _placedObjects.Add(placedObject);
        }

        public bool CanPlaceObject(Vector3 position)
        {
            return _placedObjects.All(o => !(Vector3.Distance(o.transform.position, position) < 0.5));
        }

        private readonly List<GameObject> _placedObjects = new List<GameObject>();
    }
}
