using System;
using Assets.Scripts.Components.Gui;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers.Gui
{
    public class LoadGuiManager
    {
        public LoadGuiManager(
            LoadGui loadGui,
            SaveManager saveManager)
        {
            _loadGui = loadGui;
            _saveManager = saveManager;
        }

        public void Initialize(Settings settings)
        {
            _loadGui.SetActive(false);
            _loadGuiActiveState = false;
            _loadGui.LoadBtn.onClick.AddListener(Load);
            _loadGui.CloseBtn.onClick.AddListener(Close);
            settings.LoadGuiBtn.onClick.AddListener(ToggleLoadGui);
        }

        private void ToggleLoadGui()
        {
            _loadGuiActiveState = !_loadGuiActiveState;
            _loadGui.SetActive(_loadGuiActiveState);
            if (!_loadGuiActiveState) return;
            _loadGui.SetSaveList(_saveManager.GetSaveList());
        }

        private void Load()
        {
            if (string.IsNullOrEmpty(_loadGui.LoadTxt.text))
            {
                Debug.Log("Please give a load name.");
                return;
            }
            _saveManager.Load(_loadGui.LoadTxt.text);
            ToggleLoadGui();
        }

        private void Close()
        {
            if (_loadGuiActiveState) ToggleLoadGui();
        }

        private bool _loadGuiActiveState;
        private readonly LoadGui _loadGui;
        private readonly SaveManager _saveManager;

        [Serializable]
        public class Settings
        {
            public Button LoadGuiBtn;
        }
    }
}
