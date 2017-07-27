using System;
using Assets.Scripts.Gui.Components;
using Assets.Scripts.Gui.Services;
using Assets.Scripts.Managers;
using UnityEngine;

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

        public void Initialize(Action onComplete)
        {
            _loadGui.SetActive(false);
            _loadGui.Initialize(_btnOptionFactory);
            _loadGui.LoadBtn.onClick.AddListener(() =>
            {
                Load(onComplete);
            });
            _loadGui.CloseBtn.onClick.AddListener(() =>
            {
                SetActive(false);
                onComplete();
            });
        }

        public void SetActive(bool state)
        {
            _loadGui.SetActive(state);
            if (!state) return;
            _loadGui.SetSaveList(_saveManager.GetSaveList());
        }

        private void Load(Action onComplete)
        {
            if (string.IsNullOrEmpty(_loadGui.LoadTxt.text))
            {
                Debug.Log("Please give a load name.");
                return;
            }
            _saveManager.Load(_loadGui.LoadTxt.text);
            SetActive(false);
            onComplete();
        }

        private readonly LoadGui _loadGui;
        private readonly SaveManager _saveManager;
        private readonly BtnOptionFactory _btnOptionFactory;
    }
}
