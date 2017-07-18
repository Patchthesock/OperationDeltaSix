using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Service
{
    public class LocalFilePersistance
    {
        public void Save(SaveManager.SaveModel model)
        {
            SaveDataToFile(model.Name, "Dominos", model.Dominos);
            SaveDataToFile(model.Name, "DominoProps", model.DominoProps);
        }

        public SaveManager.SaveModel Load(string saveName)
        {
            if (string.IsNullOrEmpty(saveName)) return null;
            return new SaveManager.SaveModel
            {
                Name = saveName,
                Dominos = LoadDataFromFile(saveName, "Dominos").ToList(),
                DominoProps = LoadDataFromFile(saveName, "DominoProps").ToList()
            };
        }

        public IEnumerable<string> GetSaveList()
        {
            if (!Directory.Exists("Saves/")) return new List<string>();
            return ProcessSaveList(Directory.GetDirectories("Saves/"));
        }

        private static IEnumerable<SaveManager.SaveObject> LoadDataFromFile(string saveName, string objectListName)
        {
            if (!Directory.Exists("Saves")) return new List<SaveManager.SaveObject>();
            if (!Directory.Exists("Saves/" + saveName)) return new List<SaveManager.SaveObject>();
            if (!File.Exists("Saves/" + saveName + "/" + objectListName + ".domino")) return new List<SaveManager.SaveObject>();

            var file = File.Open("Saves/" + saveName + "/" + objectListName + ".domino", FileMode.Open);
            var formatter = new BinaryFormatter();
            var objs = (ListObjectPositionSave) formatter.Deserialize(file);
            file.Close();

            return objs.ObjectPositionSave.Select(d => new SaveManager.SaveObject
            {
                Name = d.Name,
                Position = new Vector3(d.PosX, d.PosY, d.PosZ),
                Rotation = Quaternion.Euler(new Vector3(d.RotX, d.RotY, d.RotZ))
            }).ToList();
        }

        private static void SaveDataToFile(string saveName, string objectListName, IEnumerable<SaveManager.SaveObject> objects)
        {
            if (!Directory.Exists("Saves")) Directory.CreateDirectory("Saves");
            if (!Directory.Exists("Saves/" + saveName)) Directory.CreateDirectory("Saves/" + saveName);
            
            var file = File.Create("Saves/" + saveName + "/" + objectListName + ".domino");
            var formatter = new BinaryFormatter();
            formatter.Serialize(file, new ListObjectPositionSave(objects.Select(d => new ObjectPositionSave(d.Name, d.Position, d.Rotation)).ToList()));
            file.Close();
        }

        private static IEnumerable<string> ProcessSaveList(IList<string> saveList)
        {
            for (var i = 0; i < saveList.Count; i++) saveList[i] = saveList[i].Split('/')[1];
            return saveList;
        }

        [Serializable]
        private class ObjectPositionSave
        {
            public float PosX;
            public float PosY;
            public float PosZ;

            public float RotX;
            public float RotY;
            public float RotZ;

            public string Name;

            public ObjectPositionSave(string name, Vector3 position, Quaternion rotation)
            {
                Name = name;

                PosX = position.x;
                PosY = position.y;
                PosZ = position.z;

                var rot = rotation.eulerAngles;
                RotX = rot.x;
                RotY = rot.y;
                RotZ = rot.z;
            }
        }

        [Serializable]
        private class ListObjectPositionSave
        {
            // ReSharper disable once MemberHidesStaticFromOuterClass
            // ReSharper disable once FieldCanBeMadeReadOnly.Local
            public List<ObjectPositionSave> ObjectPositionSave;

            public ListObjectPositionSave(List<ObjectPositionSave> model)
            {
                ObjectPositionSave = model;
            }
        }
    }
}
