using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlacedDominoManager : MonoBehaviour
    {
        public GameObject ARBoard;
        public List<GameObject> Dominos;

        public void PlaceDomino(Vector3 position, Quaternion rotation)
        {
            GameObject objectToPlace;
            if (_nonActiveDominos.Count > 0)
            {
                objectToPlace = _nonActiveDominos.First();
                _nonActiveDominos.Remove(objectToPlace);
                objectToPlace.SetActive(true);
            }
            else objectToPlace = Instantiate(Functions.PickRandomObject(Dominos));
            
            objectToPlace.GetComponentInChildren<Rigidbody>().isKinematic = true;
            objectToPlace.GetComponentInChildren<Rigidbody>().useGravity = false;
            objectToPlace.transform.position = position;
            objectToPlace.transform.rotation = rotation;
            objectToPlace.transform.SetParent(ARBoard.transform);
            if (_placedDominos.Contains(objectToPlace)) return;
            _placedDominos.Add(objectToPlace);
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

        private readonly List<GameObject> _placedDominos = new List<GameObject>();
        private readonly List<GameObject> _nonActiveDominos = new List<GameObject>();

        [HideInInspector]
        public static PlacedDominoManager Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
}
