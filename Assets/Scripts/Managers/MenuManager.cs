using System;
using System.Collections.Generic;
using Assets.Scripts.Gui;
using Assets.Scripts.Gui.Components;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class MenuManager : ITickable
    {
        public MenuManager(
            Settings settings,
            MainMenuGui mainMenuGui,
            RemovalManager removalManager,
            SaveGuiManager saveGuiManager,
            LoadGuiManager loadGuiManager,
            PlacementManager placementManager,
            PlacedDominoManager placedDominoManager)
        {
            _settings = settings;
            _mainMenuGui = mainMenuGui;
            _removalManager = removalManager;
            _saveGuiManager = saveGuiManager;
            _loadGuiManager = loadGuiManager;
            _loadGuiManager = loadGuiManager;
            _placementManager = placementManager;
            _placedDominoManager = placedDominoManager;
        }

        public void Initialize()
        {
            SetupButtons();
            _menuState = false;
            _saveGuiManager.Initialize();
            _mainMenuGui.SetActive(false);
            _loadGuiManager.Initialize(_settings.LoadSettings);
        }

        public void Tick()
        {
            if (Input.GetKeyDown(_settings.MenuToggleKey)) ToggleMenu();
        }

        public void SubToOnMenuToggle(Action<bool> onMenuToggle)
        {
            if (_onMenuToggleListeners.Contains(onMenuToggle)) return;
            _onMenuToggleListeners.Add(onMenuToggle);
        }

        private void ToggleMenu()
        {
            _menuState = !_menuState;
            _mainMenuGui.SetActive(_menuState);
            foreach (var l in _onMenuToggleListeners) l(_menuState);
        }



        private void Create(IPlacementable model)
        {
            if (model == null) Debug.Log("Missing Prefab");
            else
            {
                _removalManager.SetActive(false);
                _placementManager.OnCreate(model);
            }
        }

        private void SetupButton(PlaceableBtn domino)
        {
            var btn = domino;
            if (domino == null) return;
            if (domino.Btn == null || domino.Model == null) return;
            domino.Btn.onClick.AddListener(() =>
            {
                ToggleMenu();
                Create(btn.Model.GetComponent<IPlacementable>());
            });
        }

        private void SetupButtons()
        {
            _settings.SaveGuiBtn.onClick.AddListener(() =>
            {
                ToggleMenu();
                _saveGuiManager.ToggleSaveGui();
            });
            _settings.LoadGuiBtn.onClick.AddListener(() =>
            {
                ToggleMenu();
                //_loadGuiManager
            });
            _settings.PropBtn.onClick.AddListener(() =>
            {
                _mainMenuGui.SetPropMenuActive(true);
                _mainMenuGui.SetPatternMenuActive(false);
            });
            _settings.PatternBtn.onClick.AddListener(() =>
            {
                _mainMenuGui.SetPropMenuActive(false);
                _mainMenuGui.SetPatternMenuActive(true);
            });
            _settings.ClearDominosBtn.onClick.AddListener(() =>
            {
                ToggleMenu();
                _placementManager.DestroyGhost();
                _placedDominoManager.RemoveDomino();
            });
            _settings.RemoveBtn.onClick.AddListener(() =>
            {
                ToggleMenu();
                _removalManager.SetActive(true);
                _placementManager.DestroyGhost();
            });

            foreach (var btn in _mainMenuGui.PatternBtns) SetupButton(btn.GetPlaceableBtn());
        }

        private bool _menuState;
        private readonly Settings _settings;
        private readonly MainMenuGui _mainMenuGui;
        private readonly RemovalManager _removalManager;
        private readonly SaveGuiManager _saveGuiManager;
        private readonly LoadGuiManager _loadGuiManager;
        private readonly PlacementManager _placementManager;
        private readonly PlacedDominoManager _placedDominoManager;
        private readonly List<Action<bool>> _onMenuToggleListeners = new List<Action<bool>>();

        [Serializable]
        public class Settings
        {
            public KeyCode MenuToggleKey;

            // Sub Menu
            public Button PropBtn;
            public Button PatternBtn;

            // Clear
            public Button RemoveBtn;
            public Button ClearDominosBtn;

            // Save & Load
            public Button SaveGuiBtn;
            public Button LoadGuiBtn;

            // Gui Settings
            //public SaveGuiManager.Settings SaveSettings;
            public LoadGuiManager.Settings LoadSettings;
        }
    }
}
