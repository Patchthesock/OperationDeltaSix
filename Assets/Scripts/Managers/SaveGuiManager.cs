using System;
using Assets.Scripts.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class SaveGuiManager
    {
        public SaveGuiManager(
            SaveGui saveGui,
            LoadGui loadGui,
            SaveManager saveManager)
        {
            _saveGui = saveGui;
            _loadGui = loadGui;
            _saveManager = saveManager;   
        }

        public void Initialize(Settings settings)
        {
            _saveGuiActiveState = false;
            _loadGuiActiveState = false;
            _saveGui.SaveBtn.onClick.AddListener(Save);
            _loadGui.LoadBtn.onClick.AddListener(Load);
            _saveGui.CloseBtn.onClick.AddListener(Close);
            _loadGui.CloseBtn.onClick.AddListener(Close);
            //settings.ResetBtn.onClick.AddListener(Reset);
            settings.SaveGuiBtn.onClick.AddListener(ToggleSaveGui);
            settings.LoadGuiBtn.onClick.AddListener(ToggleLoadGui);
        }

        private void ToggleSaveGui()
        {
            _saveGuiActiveState = !_saveGuiActiveState;
            _saveGui.SetActive(_saveGuiActiveState);
            if (!_saveGuiActiveState) return;
            _saveGui.SetSaveList(_saveManager.GetSaveList());
        }

        private void ToggleLoadGui()
        {
            _loadGuiActiveState = !_loadGuiActiveState;
            _loadGui.SetActive(_loadGuiActiveState);
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_saveGui.SaveTxt.text))
            {
                Debug.Log("Please give a save name.");
                return;
            }
            ToggleSaveGui();
            _saveManager.Save(_saveGui.SaveTxt.text);
        }

        private void Load()
        {
            if (string.IsNullOrEmpty(_loadGui.LoadTxt.text))
            {
                Debug.Log("Please give a load name.");
                return;
            }
            ToggleLoadGui();
            _saveManager.Load(_loadGui.LoadTxt.text);
        }

        private void Reset()
        {
            //_saveManager.Load("auto");
        }

        private void Close()
        {
            if (_saveGuiActiveState) ToggleSaveGui();
            if (_loadGuiActiveState) ToggleLoadGui();
        }

        private bool _saveGuiActiveState;
        private bool _loadGuiActiveState;

        private readonly SaveGui _saveGui;
        private readonly LoadGui _loadGui;
        private readonly SaveManager _saveManager;

        [Serializable]
        public class Settings
        {
            //public Button ResetBtn;
            public Button SaveGuiBtn;
            public Button LoadGuiBtn;
        }
    }
}
