using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components;
using Assets.Scripts.Gui.Components;
using UnityEngine;

namespace Assets.Scripts.Gui.Services
{
    public class BtnOptionFactory
    {
        public BtnOptionFactory(
            Settings settings,
            PrefabFactory prefabFactory)
        {
            _settings = settings;
            _prefabFactory = prefabFactory;
        }

        public void Clear(string selectId)
        {
            if (!_activeSaveOptions.ContainsKey(selectId)) return;
            foreach (var i in _activeSaveOptions[selectId])
            {
                _nonActiveSaveOptions.Add(i);
            }
            _activeSaveOptions.Remove(selectId);
        }

        public BtnOption Create(string selectId)
        {
            var option = _nonActiveSaveOptions.Count > 0 ? GetExistingSaveOption() : GetNewSaveOption();
            if (_activeSaveOptions.ContainsKey(selectId)) _activeSaveOptions[selectId].Add(option);
            else _activeSaveOptions.Add(selectId, new List<BtnOption>
            {
                option
            });
            return option;
        }

        private BtnOption GetNewSaveOption()
        {
            return _prefabFactory.Instantiate(_settings.BtnOptionPrefab).GetComponent<BtnOption>();
        }

        private BtnOption GetExistingSaveOption()
        {
            var i = _nonActiveSaveOptions.First();
            _nonActiveSaveOptions.Remove(i);
            return i;
        }

        private readonly Settings _settings;
        private readonly PrefabFactory _prefabFactory;
        private readonly List<BtnOption> _nonActiveSaveOptions = new List<BtnOption>();
        private readonly Dictionary<string, List<BtnOption>> _activeSaveOptions = new Dictionary<string, List<BtnOption>>();

        [Serializable]
        public class Settings
        {
            public GameObject BtnOptionPrefab;
        }
    }
}
