using System;
using Assets.Scripts.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers.Gui
{
    public class SaveGuiManager
    {
        public SaveGuiManager(
            SaveGui saveGui,
            SaveManager saveManager)
        {
            _saveGui = saveGui;
            _saveManager = saveManager;
        }

        public void Initialize(Settings settings)
        {
            _saveGuiActiveState = false;
            _saveGui.SaveBtn.onClick.AddListener(Save);
            _saveGui.CloseBtn.onClick.AddListener(Close);
            settings.SaveGuiBtn.onClick.AddListener(ToggleSaveGui);
        }

        private void ToggleSaveGui()
        {
            _saveGuiActiveState = !_saveGuiActiveState;
            _saveGui.SetActive(_saveGuiActiveState);
            if (!_saveGuiActiveState) return;
            _saveGui.SetSaveList(_saveManager.GetSaveList());
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
            _saveGui.SaveTxt.text = "I worked";
        }

        private void Close()
        {
            if (_saveGuiActiveState) ToggleSaveGui();
        }

        private bool _saveGuiActiveState;

        private readonly SaveGui _saveGui;
        private readonly SaveManager _saveManager;

        [Serializable]
        public class Settings
        {
            public Button SaveGuiBtn;
        }
    }
}
