using System.Collections.Generic;
using System.Linq;
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
                Dominos = GameObjectToSaveObject(_placedDominoManager.GetPlacedDominos()),
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
            _placedObjectManager.AddObject(model.Dominos);
            _placedDominoManager.PlaceDomino(model.DominoProps);
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

        public class SaveObject
        {
            public string Name;
            public Vector3 Position;
            public Quaternion Rotation;
        }
    }
}
