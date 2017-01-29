using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class MenuController
    {
        public MenuController(
            Settings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            foreach (var m in _settings.MenuOptions)
            {
                var m1 = m;
                if (m.Button == null) continue;
                m.Button.onClick.AddListener(() =>
                {
                    if (m1.GameObject == null) return;
                    OnItemSelected(m1.GameObject);
                });
            }
        }

        public void SubscribeToOnItemSelected(Action<GameObject> onItemSelected)
        {
            if (_onItemSelectedListeners.Contains(onItemSelected)) return;
            _onItemSelectedListeners.Add(onItemSelected);
        }

        private void OnItemSelected(GameObject model)
        {
            if (model == null) return;
            foreach (var l in _onItemSelectedListeners)
            {
                l(model);
            }
        }

        private readonly Settings _settings;
        private readonly List<Action<GameObject>> _onItemSelectedListeners = new List<Action<GameObject>>();

        [Serializable]
        public class Settings
        {
            public List<MenuOption> MenuOptions;
        }

        [Serializable]
        public class MenuOption
        {
            public Button Button;
            public GameObject GameObject;
        }
    }
}
