using System;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager
    {
        public GameManager(
            Menu menu,
            Settings settings,
            SaveManager saveManager,
            PlacementManager placementManager,
            PlacedDominoManager placedDominoManager)
        {
            _settings = settings;
            _saveManager = saveManager;
            _placementManager = placementManager;
            _placedDominoManager = placedDominoManager;

            PlayControl(false);
            menu.PlayBtn.onClick.AddListener(() => { PlayControl(!_isPlaying); });
        }

        private void PlayControl(bool state)
        {
            _isPlaying = state;
            if (state) _saveManager.Save(); // Autosave when in play
            _placementManager.SetActive(!state);
            _settings.HandObject.SetActive(state);
            _placedDominoManager.SetDominoPhysics(state);
            if (state) Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Physics.gravity = state ? new Vector3(0, _settings.Gravity, 0) : new Vector3(0, 0, 0);
        }

        private bool _isPlaying;
        private readonly Settings _settings;
        private readonly SaveManager _saveManager;
        private readonly PlacementManager _placementManager;
        private readonly PlacedDominoManager _placedDominoManager;

        [Serializable]
        public class Settings
        {
            public float Gravity = -50;
            public GameObject HandObject;
        }
    }
}
