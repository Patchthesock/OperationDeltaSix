using System;
using System.Collections.Generic;
using Assets.Scripts.Gui;
using Assets.Scripts.Gui.Components;
using Assets.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class MenuManager : ITickable
    {
        public MenuManager(
            Settings settings,
            RemovalManager removalManager,
            SaveGuiManager saveGuiManager,
            LoadGuiManager loadGuiManager,
            PlacementManager placementManager,
            PlacedDominoManager placedDominoManager)
        {
            _settings = settings;
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
            _saveGuiManager.Initialize(_settings.SaveSettings);
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
            _settings.MainUI.SetActive(_menuState);
            foreach (var l in _onMenuToggleListeners) l(_menuState);
        }

        private void SetupButtons()
        {
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
            _settings.PropBtn.onClick.AddListener(() =>
            {
                _settings.MainUI.SetPropMenuActive(true);
                _settings.MainUI.SetPatternMenuActive(false);
            });
            _settings.PatternBtn.onClick.AddListener(() =>
            {
                _settings.MainUI.SetPropMenuActive(false);
                _settings.MainUI.SetPatternMenuActive(true);
            });

            foreach (var btn in _settings.MainUI.PatternBtns) SetupButton(btn);
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

        private void SetupButton(Pl domino)
        {
            var btn = domino;
            if (domino.SelectButton == null || domino.Placeable == null) return;
            domino.SelectButton.onClick.AddListener(() =>
            {
                ToggleMenu();
                Create(btn.Placeable.GetComponent<IPlacementable>());
            });
        }

        private bool _menuState;
        private readonly Settings _settings;
        private readonly RemovalManager _removalManager;
        private readonly SaveGuiManager _saveGuiManager;
        private readonly LoadGuiManager _loadGuiManager;
        private readonly PlacementManager _placementManager;
        private readonly PlacedDominoManager _placedDominoManager;
        private readonly List<Action<bool>> _onMenuToggleListeners = new List<Action<bool>>();

        [Serializable]
        public class Settings
        {
            public MainUI MainUI;
            public KeyCode MenuToggleKey;

            // Sub Menu
            public Button PropBtn;
            public Button PatternBtn;

            // Clear
            public Button RemoveBtn;
            public Button ClearDominosBtn;

            // Gui Settings
            public SaveGuiManager.Settings SaveSettings;
            public LoadGuiManager.Settings LoadSettings;
        }
    }
}
