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
                PlacementManager.instance.PlaceObject(_placedObjects);
            });
        }

        public void Save()
        {
            Save(PlacedObjectManager.instance.GetPlacedObjects().ToList().Select(t => new ObjectPosition
            {
                Position = t.transform.position + new Vector3(0, -1f, 0),
                Rotation = t.transform.rotation
            }).ToList());
        }

        private void Save(IEnumerable<ObjectPosition> positions)
        {
            _placedObjects = positions.ToList();
        }

        private List<ObjectPosition> _placedObjects = new List<ObjectPosition>();

        public class ObjectPosition
        {
            public Vector3 Position;
            public Quaternion Rotation;
        }
    }
}
