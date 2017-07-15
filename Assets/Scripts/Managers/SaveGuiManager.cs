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
            Settings settings,
            SaveManager saveManager)
        {
            _saveGui = saveGui;
            _loadGui = loadGui;
            _saveManager = saveManager;
            _saveGuiActiveState = false;
            _loadGuiActiveState = false;

            saveGui.SaveBtn.onClick.AddListener(Save);
            loadGui.LoadBtn.onClick.AddListener(Load);
            settings.ResetBtn.onClick.AddListener(Reset);
            settings.SaveGuiBtn.onClick.AddListener(ToggleSaveGui);
            settings.LoadGuiBtn.onClick.AddListener(ToggleLoadGui);
        }

        private void ToggleSaveGui()
        {
            _saveGuiActiveState = !_saveGuiActiveState;
            _saveGui.SetActive(_saveGuiActiveState);
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
            _saveManager.Save(_saveGui.SaveTxt.text);
        }

        private void Load()
        {
            if (string.IsNullOrEmpty(_loadGui.LoadTxt.text))
            {
                Debug.Log("Please give a load name.");
                return;
            }
            _saveManager.Load(_loadGui.LoadTxt.text);
        }

        private void Reset()
        {
            _saveManager.Load("auto");
        }

        private bool _saveGuiActiveState;
        private bool _loadGuiActiveState;

        private readonly SaveGui _saveGui;
        private readonly LoadGui _loadGui;
        private readonly SaveManager _saveManager;

        [Serializable]
        public class Settings
        {
            public Button ResetBtn;
            public Button SaveGuiBtn;
            public Button LoadGuiBtn;
        }
    }
}
