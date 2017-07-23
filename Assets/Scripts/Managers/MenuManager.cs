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
            foreach (var l in _onMenuToggleListeners) l(_menuState);
        }

        private void SetupButtons()
        {
            _settings.ClearDominosBtn.onClick.AddListener(() =>
            {
                _placementManager.DestroyGhost();
                _placedDominoManager.RemoveDomino();
            });
            _settings.RemoveBtn.onClick.AddListener(() =>
            {
                _removalManager.SetActive(true);
                _placementManager.DestroyGhost();
            });

            foreach (var btn in _settings.Domino) SetupButton(btn);
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

        private void SetupButton(SelectableDomino domino)
        {
            var btn = domino;
            if (domino.SelectButton == null || domino.Placeable == null) return;
            domino.SelectButton.onClick.AddListener(() => { Create(btn.Placeable.GetComponent<IPlacementable>()); });
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
        public class SelectableDomino
        {
            public Button SelectButton;
            public GameObject Placeable;
        }

        [Serializable]
        public class Settings
        {
            public KeyCode MenuToggleKey;
            public MainUI MainUI;
            public GameObject PropsInventory;
            public GameObject DominoesInventory;

            // Clear
            public Button RemoveBtn;
            public Button ClearDominosBtn;

            // Dominos
            public List<SelectableDomino> Domino;

            // Gui Settings
            public SaveGuiManager.Settings SaveSettings;
            public LoadGuiManager.Settings LoadSettings;
        }
    }
}
