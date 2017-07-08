using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class SaveManager
    {
        public SaveManager(
            Settings settings,
            PlacedDominoManager placedDominoManager,
            PlacedDominoPropManager placedObjectManager)
        {
            _placedDominoManager = placedDominoManager;
            _placedObjectManager = placedObjectManager;
            settings.SaveBtn.onClick.AddListener(Save);
            settings.ResetBtn.onClick.AddListener(() =>
            {
                _placedObjectManager.AddObject(_placedObjects);
                _placedDominoManager.PlaceDomino(_placedDominos);
            });
            LoadData();
        }

        public void Save()
        {
            _placedObjects.Clear();
            _placedObjects.AddRange(_placedObjectManager.GetPlacedObjects().ToList().Select(t => new ObjectPosition
            {
                GameObject = t,
                Position = t.transform.position,
                Rotation = t.transform.rotation
            }).ToList());
            _placedDominos.Clear();
            _placedDominos.AddRange(_placedDominoManager.GetPlacedDominos().ToList().Select(t => new ObjectPosition
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
            if (!File.Exists("Saves/dominos.binary")) return;
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

        private readonly PlacedDominoManager _placedDominoManager;
        private readonly PlacedDominoPropManager _placedObjectManager;

        [Serializable]
        public class Settings
        {
            public Button SaveBtn;
            public Button ResetBtn;
        }
    }
}
