using System;
using System.Collections.Generic;
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

        public void Initialize(bool isPlaying)
        {
            _gameState = isPlaying;
            ChangeState(_gameState);
            if (_settings.PlayBtn == null) return;
            _settings.PlayBtn.onClick.AddListener(() =>
            {
                ChangeState(!_gameState);
            });
        }

        public void SubscribeToOnGameStateChanged(Action<bool> onGameStateChanged)
        {
            if (_onGameStateChangedListeners.Contains(onGameStateChanged)) return;
            _onGameStateChangedListeners.Add(onGameStateChanged);
        }

        private void OnGameStateChanged(bool state)
        {
            foreach (var l in _onGameStateChangedListeners)
            {
                l(state);
            }
        }

        private void ChangeState(bool state)
        {
            _gameState = state;
            OnGameStateChanged(_gameState);
        }

        private bool _gameState;
        private readonly Settings _settings;
        private readonly List<Action<bool>> _onGameStateChangedListeners = new List<Action<bool>>();

        [Serializable]
        public class Settings
        {
            public Button PlayBtn;
        }
    }
}
