using System;
using Assets.Scripts.Gui.Components;
using Assets.Scripts.Gui.Services;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class LoadGuiManager
    {
        public LoadGuiManager(
            LoadGui loadGui,
            SaveManager saveManager,
            BtnOptionFactory btnOptionFactory)
        {
            _loadGui = loadGui;
            _saveManager = saveManager;
            _btnOptionFactory = btnOptionFactory;
        }

        public void Initialize(Settings settings)
        {
            _loadGui.Initialize(_btnOptionFactory);
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
        private readonly BtnOptionFactory _btnOptionFactory;

        [Serializable]
        public class Settings
        {
            public Button LoadGuiBtn;
        }
    }
}
