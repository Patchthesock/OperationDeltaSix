using System;
using Assets.Scripts.Factories;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class LevelController
    {
        public LevelController(
            Settings settings,
            PrefabFactory prefabFactory)
        {
            _settings = settings;
            _prefabFactory = prefabFactory;
        }

        public void Initialize()
        {
            //_prefabFactory.Create(_settings.Level);
        }

        private readonly Settings _settings;
        private readonly PrefabFactory _prefabFactory;

        [Serializable]
        public class Settings
        {
            public GameObject Level;
        }
    }
}
