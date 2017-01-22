using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
            LoadData();
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

            SaveDataToFile(_placedDominos, _placedObjects);
        }

        private List<ObjectPosition> _placedObjects = new List<ObjectPosition>();
        private List<ObjectPosition> _placedDominos = new List<ObjectPosition>();

        public class ObjectPosition
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public GameObject GameObject;
        }

        [Serializable]
        public class ObjectPositionSave
        {
            public float PosX;
            public float PosY;
            public float PosZ;

            public float RotX;
            public float RotY;
            public float RotZ;

            public ObjectPositionSave(Vector3 position, Quaternion rotation)
            {
                PosX = position.x;
                PosY = position.y;
                PosZ = position.z;

                var rot = rotation.eulerAngles;
                RotX = rot.x;
                RotY = rot.y;
                RotZ = rot.z;
            }

            public ObjectPositionSave() { }
        }

        [Serializable]
        public class ListObjectPositionSave
        {
            public List<ObjectPositionSave> ObjectPositionSave;
        }

        private static void SaveDataToFile(IEnumerable<ObjectPosition> dominos, List<ObjectPosition> objects)
        {
            var data = new ListObjectPositionSave
            {
                ObjectPositionSave = dominos.Select(d => new ObjectPositionSave(d.Position, d.Rotation)).ToList()
            };
            if (!Directory.Exists("Saves")) Directory.CreateDirectory("Saves");
            var formatter = new BinaryFormatter();
            var dominosFile = File.Create("Saves/dominos.binary");
            //var objectsFile = File.Create("saves/objects.binary");
            formatter.Serialize(dominosFile, data);
            //formatter.Serialize(objectsFile, objects);
            dominosFile.Close();
            //objectsFile.Close();
        }

        private void LoadData()
        {
            if (!Directory.Exists("Saves")) return;
            var formatter = new BinaryFormatter();
            var dominosFile = File.Open("Saves/dominos.binary", FileMode.Open);
            //var objectsFile = File.Open("Saves/objects.binary", FileMode.Open);
            var dom = (ListObjectPositionSave)formatter.Deserialize(dominosFile);
            //_placedObjects = (List<ObjectPosition>)formatter.Deserialize(objectsFile);
            dominosFile.Close();
            //objectsFile.Close();

            _placedDominos = dom.ObjectPositionSave.Select(d => new ObjectPosition
            {
                GameObject = null,
                Position = new Vector3(d.PosX, d.PosY, d.PosZ),
                Rotation = Quaternion.Euler(new Vector3(d.RotX, d.RotY, d.RotZ))
            }).ToList();
        }
    }
}
