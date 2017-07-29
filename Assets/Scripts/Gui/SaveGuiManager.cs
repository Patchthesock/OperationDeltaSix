using System;
using Assets.Scripts.Gui.Components;
using Assets.Scripts.Gui.Services;
using Assets.Scripts.Managers;
using UnityEngine;

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

        public bool CurrentState { get; private set; }

        public void Initialize(Action onComplete)
        {
            CurrentState = false;
            _saveGui.SetActive(false);
            _saveConfirmGui.SetActive(false);
            _saveGui.Initialize(_btnOptionFactory);
            _saveGui.SaveBtn.onClick.AddListener(Save);
            
            _saveConfirmGui.Initialize(() =>
            {
                SaveConfirmed(onComplete);
            }, () =>
            {
                SaveCancelled(onComplete);
            });
            _saveGui.CloseBtn.onClick.AddListener(() => {
                SetActive(false);
                onComplete();
            });
        }

        public void SetActive(bool state)
        {
            CurrentState = state;
            _saveGui.SetActive(state);
            if (!CurrentState) return;
            _saveGui.SetSaveList(_saveManager.GetSaveList());
        }

        private void ToggleSaveConfirmGui()
        {
            CurrentState = !CurrentState;
            _saveConfirmGui.SetActive(CurrentState);
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_saveGui.SaveTxt.text))
            {
                Debug.Log("Please give a save name.");
                return;
            }
            SetActive(false);
            ToggleSaveConfirmGui();
        }

        private void SaveConfirmed(Action onComplete)
        {
            _saveManager.Save(_saveGui.SaveTxt.text);
            ToggleSaveConfirmGui();
            onComplete();
        }

        private void SaveCancelled(Action onComplete)
        {
            ToggleSaveConfirmGui();
            onComplete();
        }

        private readonly SaveGui _saveGui;
        private readonly SaveManager _saveManager;
        private readonly SaveConfirmGui _saveConfirmGui;
        private readonly BtnOptionFactory _btnOptionFactory;
    }
}
