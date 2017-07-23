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
            SaveConfirmGui saveConfirmGui,
            BtnOptionFactory btnOptionFactory)
        {
            _saveGui = saveGui;
            _saveManager = saveManager;
            _saveConfirmGui = saveConfirmGui;
            _btnOptionFactory = btnOptionFactory;
        }

        public void Initialize(Settings settings)
        {
            _saveGuiState = false;
            _saveGui.SetActive(false);
            _saveConfirmGuiState = false;
            _saveConfirmGui.SetActive(false);
            _saveGui.Initialize(_btnOptionFactory);
            _saveGui.SaveBtn.onClick.AddListener(Save);
            _saveGui.CloseBtn.onClick.AddListener(Close);
            settings.SaveGuiBtn.onClick.AddListener(ToggleSaveGui);
            _saveConfirmGui.Initialize(SaveConfirmed, ToggleSaveConfirmGui);
        }

        private void ToggleSaveGui()
        {
            _saveGuiState = !_saveGuiState;
            _saveGui.SetActive(_saveGuiState);
            if (!_saveGuiState) return;
            _saveGui.SetSaveList(_saveManager.GetSaveList());
        }

        private void ToggleSaveConfirmGui()
        {
            _saveConfirmGuiState = !_saveConfirmGuiState;
            _saveConfirmGui.SetActive(_saveConfirmGuiState);
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_saveGui.SaveTxt.text))
            {
                Debug.Log("Please give a save name.");
                return;
            }
            ToggleSaveGui();
            ToggleSaveConfirmGui();
        }

        private void SaveConfirmed()
        {
            _saveManager.Save(_saveGui.SaveTxt.text);
        }

        private void Close()
        {
            if (_saveGuiState) ToggleSaveGui();
        }

        private bool _saveGuiState;
        private bool _saveConfirmGuiState;
        private readonly SaveGui _saveGui;
        private readonly SaveManager _saveManager;
        private readonly SaveConfirmGui _saveConfirmGui;
        private readonly BtnOptionFactory _btnOptionFactory;

        [Serializable]
        public class Settings
        {
            public Button SaveGuiBtn;
        }
    }
}
