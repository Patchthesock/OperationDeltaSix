using System;
using System.Collections.Generic;
using Assets.Scripts.Factories;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class GameStateController
    {
        public GameStateController(
            Settings settings)
        {
            _settings = settings;
        }

        public void Initialize(GameStateFactory.GameStates initialState)
        {
            _gameState = initialState;
            ChangeState(_gameState);
            if (_settings.PlayBtn == null) return;
            _settings.PlayBtn.onClick.AddListener(() =>
            {
                ChangeState(
                    _gameState == GameStateFactory.GameStates.Build
                    ? GameStateFactory.GameStates.Play
                    : GameStateFactory.GameStates.Build);
            });
        }

        public void SubscribeToOnGameStateChanged(Action<GameStateFactory.GameStates> onGameStateChanged)
        {
            if (_onGameStateChangedListeners.Contains(onGameStateChanged)) return;
            _onGameStateChangedListeners.Add(onGameStateChanged);
        }

        private void OnGameStateChanged(GameStateFactory.GameStates state)
        {
            foreach (var l in _onGameStateChangedListeners)
            {
                l(state);
            }
        }

        private void ChangeState(GameStateFactory.GameStates state)
        {
            _gameState = state;
            OnGameStateChanged(_gameState);
        }

        private GameStateFactory.GameStates _gameState;
        private readonly Settings _settings;
        private readonly List<Action<GameStateFactory.GameStates>> _onGameStateChangedListeners = new List<Action<GameStateFactory.GameStates>>();

        [Serializable]
        public class Settings
        {
            public Button PlayBtn;
        }
    }
}
