﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Service;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SaveManager
    {
        public SaveManager(
            PlacedDominoManager placedDominoManager,
            LocalFilePersistance localFilePersistance,
            PlacedDominoPropManager placedObjectManager)
        {
            _placedDominoManager = placedDominoManager;
            _placedObjectManager = placedObjectManager;
            _localFilePersistance = localFilePersistance;
        }

        public void Save(string saveName)
        {
            _localFilePersistance.Save(new SaveModel
            {
                Name = saveName,
                Dominos = _placedDominoManager.GetPlacedDominosSave().ToList(),
                DominoProps = GameObjectToSaveObject(_placedObjectManager.GetPlacedObjects())
            });
        }

        public void Load(string saveName)
        {
            var model = _localFilePersistance.Load(saveName);
            if (model == null)
            {
                Debug.Log("Load Failed");
                return;
            }
            
            if (model.Dominos.Count > 0) _placedDominoManager.PlaceDomino(model.Dominos);
            if (model.DominoProps.Count > 0) _placedObjectManager.AddObject(model.DominoProps);
        }

        public IEnumerable<string> GetSaveList()
        {
            return _localFilePersistance.GetSaveList();
        }
        
        private static List<SaveObject> GameObjectToSaveObject(IEnumerable<GameObject> model)
        {
            return model.Select(GameObjectToSaveObject).ToList();
        }

        private static SaveObject GameObjectToSaveObject(GameObject model)
        {
            return new SaveObject
            {
                Name = model.name,
                Position = model.transform.position,
                Rotation = model.transform.rotation
            };
        }

        private readonly PlacedDominoManager _placedDominoManager;
        private readonly LocalFilePersistance _localFilePersistance;
        private readonly PlacedDominoPropManager _placedObjectManager;

        public class SaveModel
        {
            public string Name;
            public List<SaveObject> Dominos;
            public List<SaveObject> DominoProps;
        }
    }
}
