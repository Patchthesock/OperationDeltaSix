﻿using System;
using Assets.Scripts.Components.GameModels;
using Assets.Scripts.Gui;
using Assets.Scripts.Interfaces;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class MenuManager : IInitializable
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
            _saveGuiManager.Initialize(_settings.SaveSettings);
            _loadGuiManager.Initialize(_settings.LoadSettings);
        }

        private void SetupButtons()
        {
            _settings.NintyLeft.SelectButton.onClick.AddListener(() => { Create(_settings.NintyLeft.Dominos); });
            _settings.NintyRight.SelectButton.onClick.AddListener(() => { Create(_settings.NintyRight.Dominos); });
            _settings.TenDominos.SelectButton.onClick.AddListener(() => { Create(_settings.TenDominos.Dominos); });
            _settings.FiveDominos.SelectButton.onClick.AddListener(() => { Create(_settings.FiveDominos.Dominos); });
            _settings.SingleDomino.SelectButton.onClick.AddListener(() => { Create(_settings.SingleDomino.Domino); });
            _settings.TwentyDominos.SelectButton.onClick.AddListener(() => { Create(_settings.TwentyDominos.Dominos); });
            _settings.OneEightyTurn.SelectButton.onClick.AddListener(() => { Create(_settings.OneEightyTurn.Dominos); });

            //_settings.LowStep.SelectButton.onClick.AddListener(() => { Create(_settings.LowStep.DominosProp); });
            //_settings.HighStep.SelectButton.onClick.AddListener(() => { Create(_settings.HighStep.DominosProp); });
            //_settings.LowBridge.SelectButton.onClick.AddListener(() => { Create(_settings.LowBridge.DominosProp); });
            //_settings.HighBridge.SelectButton.onClick.AddListener(() => { Create(_settings.HighBridge.DominosProp); });
            //_settings.DownSlide.SelectButton.onClick.AddListener(() => { Create(_settings.DownSlide.DominosProp); });
            _settings.ClearDominos.onClick.AddListener(() =>
            {
                _placementManager.DestroyGhost();
                _placedDominoManager.RemoveDomino();
            });
            _settings.RemoveBtn.onClick.AddListener(() =>
            {
                _removalManager.SetActive(true);
                _placementManager.DestroyGhost();
            });
        }

        private void Create(IPlacementable model)
        {
            if (model == null) UnityEngine.Debug.Log("Missing Prefab");
            else
            {
                _removalManager.SetActive(false);
                _placementManager.OnCreate(model);
            }
        }

        private readonly Settings _settings;
        private readonly RemovalManager _removalManager;
        private readonly SaveGuiManager _saveGuiManager;
        private readonly LoadGuiManager _loadGuiManager;
        private readonly PlacementManager _placementManager;
        private readonly PlacedDominoManager _placedDominoManager;

        [Serializable]
        public class SelectableDomino
        {
            public Domino Domino;
            public Button SelectButton;
        }

        [Serializable]
        public class SelectableDominos
        {
            public Dominos Dominos;
            public Button SelectButton;
        }

        [Serializable]
        public class SelectableDominosProp
        {
            public Button SelectButton;
            public DominosProp DominosProp;
        }

        [Serializable]
        public class Settings
        {
            // Clear
            public Button RemoveBtn;
            public Button ClearDominos;

            // Dominos
            public SelectableDomino SingleDomino;
            public SelectableDominos FiveDominos;
            public SelectableDominos TenDominos;
            public SelectableDominos TwentyDominos;
            public SelectableDominos NintyLeft;
            public SelectableDominos NintyRight;
            public SelectableDominos OneEightyTurn;

            // Props
            public SelectableDominosProp LowStep;
            public SelectableDominosProp HighStep;
            public SelectableDominosProp DownSlide;
            public SelectableDominosProp LowBridge;
            public SelectableDominosProp HighBridge;

            // Gui Settings
            public SaveGuiManager.Settings SaveSettings;
            public LoadGuiManager.Settings LoadSettings;
        }
    }
}
