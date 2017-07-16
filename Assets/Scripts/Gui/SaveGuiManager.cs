using System;
using Assets.Scripts.Gui.Components;
using Assets.Scripts.Gui.Services;
using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Gui
{
    public class SaveGuiManager
    {
        public SaveGuiManager(
            SaveGui saveGui,
            SaveManager saveManager,
            BtnOptionFactory btnOptionFactory)
        {
            _saveGui = saveGui;
            _saveManager = saveManager;
            _btnOptionFactory = btnOptionFactory;
        }

        public void Initialize(Settings settings)
        {
            _saveGui.Initialize(_btnOptionFactory);
            _saveGui.SetActive(false);
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
            _saveManager.Save(_saveGui.SaveTxt.text);
            ToggleSaveGui();
        }

        private void Close()
        {
            if (_saveGuiActiveState) ToggleSaveGui();
        }

        private bool _saveGuiActiveState;
        private readonly SaveGui _saveGui;
        private readonly SaveManager _saveManager;
        private readonly BtnOptionFactory _btnOptionFactory;

        [Serializable]
        public class Settings
        {
            public Button SaveGuiBtn;
        }
    }
}
