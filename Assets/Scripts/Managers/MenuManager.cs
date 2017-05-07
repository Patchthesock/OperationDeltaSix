using System;
using Assets.Scripts.Components.GameModels;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class MenuManager : IInitializable
    {
        public MenuManager(
            Settings settings,
            PlacementManager placementManager,
            PlacedDominoManager placedDominoManager)
        {
            _settings = settings;
            _placementManager = placementManager;
            _placedDominoManager = placedDominoManager;
        }

        public void Initialize()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            _settings.NintyLeft.SelectButton.onClick.AddListener(() => { _placementManager.OnCreate(_settings.NintyLeft.Dominos); });
            _settings.NintyRight.SelectButton.onClick.AddListener(() => { _placementManager.OnCreate(_settings.NintyRight.Dominos); });
            _settings.BridgeProp.SelectButton.onClick.AddListener(() => { _placementManager.OnCreate(_settings.BridgeProp.Dominos); });
            _settings.TenDominos.SelectButton.onClick.AddListener(() => { _placementManager.OnCreate(_settings.TenDominos.Dominos); });
            _settings.FiveDominos.SelectButton.onClick.AddListener(() => { _placementManager.OnCreate(_settings.FiveDominos.Dominos); });
            _settings.SingleDomino.SelectButton.onClick.AddListener(() => { _placementManager.OnCreate(_settings.SingleDomino.Domino); });
            _settings.TwentyDominos.SelectButton.onClick.AddListener(() => { _placementManager.OnCreate(_settings.TwentyDominos.Dominos); });
            _settings.StepSlideProp.SelectButton.onClick.AddListener(() => { _placementManager.OnCreate(_settings.StepSlideProp.Dominos); });
            _settings.OneEightyTurn.SelectButton.onClick.AddListener(() => { _placementManager.OnCreate(_settings.OneEightyTurn.Dominos); });
            _settings.ClearDominos.onClick.AddListener(() =>
            {
                _placementManager.DestroyGhost();
                _placedDominoManager.RemoveDomino();
            });
            _settings.RemoveBtn.onClick.AddListener(() =>
            {
                _placementManager.DestroyGhost();
                // TODO _removingObjects = true;
            });
        }

        private readonly Settings _settings;
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
            public SelectableDominos BridgeProp;
            public SelectableDominos StepSlideProp;

            
        }
    }
}
