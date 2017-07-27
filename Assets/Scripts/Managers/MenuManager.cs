using System;
using System.Collections.Generic;
using Assets.Scripts.Gui;
using Assets.Scripts.Gui.Components;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Models;
using UnityEngine;
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

        public void Initialize(Action onPlayBtnPress)
        {
            _menuState = false;
            SetupButtons(onPlayBtnPress);
            _mainMenuGui.SetActive(false);
            _saveGuiManager.Initialize(() => { SetActive(false); });
            _loadGuiManager.Initialize(() => { SetActive(false); });
        }

        public void Tick()
        {
            if (Input.GetKeyDown(_settings.MenuToggleKey)) SetActive(!_menuState);
        }

        public void SubToOnMenuToggle(Action<bool> onMenuToggle)
        {
            if (_onMenuToggleListeners.Contains(onMenuToggle)) return;
            _onMenuToggleListeners.Add(onMenuToggle);
        }

        private void SetActive(bool state)
        {
            _menuState = state;
            _placementManager.DestroyGhost();
            _removalManager.SetActive(false);
            _saveGuiManager.SetActive(false);
            _loadGuiManager.SetActive(false);
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
                SetActive(false);
                Create(btn.Model.GetComponent<IPlacementable>());
            });
        }

        private void SetupButtons(Action onPlayBtnPress)
        {
            _mainMenuGui.SaveGuiBtn.onClick.AddListener(() =>
            {
                _mainMenuGui.SetActive(false);
                _saveGuiManager.SetActive(true);
            });
            _mainMenuGui.LoadGuiBtn.onClick.AddListener(() =>
            {
                _mainMenuGui.SetActive(false);
                _loadGuiManager.SetActive(true);
            });
            _mainMenuGui.PropBtn.onClick.AddListener(() =>
            {
                _mainMenuGui.SetPropMenuActive(true);
                _mainMenuGui.SetPatternMenuActive(false);
            });
            _mainMenuGui.PatternBtn.onClick.AddListener(() =>
            {
                _mainMenuGui.SetPropMenuActive(false);
                _mainMenuGui.SetPatternMenuActive(true);
            });
            _mainMenuGui.ClearDominosBtn.onClick.AddListener(() =>
            {
                SetActive(false);
                _placedDominoManager.RemoveDomino();
            });
            _mainMenuGui.RemoveBtn.onClick.AddListener(() =>
            {
                SetActive(false);
                _removalManager.SetActive(true);
            });
            _mainMenuGui.PlayBtn.onClick.AddListener(() =>
            {
                SetActive(false);
                onPlayBtnPress();
            });

            foreach (var btn in _mainMenuGui.PropBtns) SetupButton(btn.GetPlaceableBtn());
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
        }
    }
}
