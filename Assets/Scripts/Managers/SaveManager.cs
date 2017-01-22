using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class SaveManager : MonoBehaviour
    {
        public Button SaveBtn;
        public Button ResetBtn;

        [HideInInspector]
        public static SaveManager instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            Init();
        }

        private void Init()
        {
            if (SaveBtn == null) return;
            SaveBtn.onClick.AddListener(Save);
            ResetBtn.onClick.AddListener(() =>
            {
                GameManager.instance.PlayControl(false);
                PlacedObjectManager.instance.AddObject(_placedObjects);
                PlacedDominoManager.instance.PlaceDomino(_placedDominos);
            });
        }

        public void Save()
        {
            _placedObjects.Clear();
            _placedObjects.AddRange(PlacedObjectManager.instance.GetPlacedObjects().ToList().Select(t => new ObjectPosition
            {
                GameObject = t,
                Position = t.transform.position,
                Rotation = t.transform.rotation
            }).ToList());
            _placedDominos.Clear();
            _placedDominos.AddRange(PlacedDominoManager.instance.GetPlacedDominos().ToList().Select(t => new ObjectPosition
            {
                Position = t.transform.position,
                Rotation = t.transform.rotation
            }));
        }

        private void Save(IEnumerable<ObjectPosition> positions)
        {
            _placedObjects = positions.ToList();
        }

        private List<ObjectPosition> _placedObjects = new List<ObjectPosition>();
        private List<ObjectPosition> _placedDominos = new List<ObjectPosition>();

        public class ObjectPosition
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public GameObject GameObject;
        }
    }
}
