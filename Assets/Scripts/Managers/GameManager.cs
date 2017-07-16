using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class GameManager : IInitializable
    {
        public GameManager(
            Settings settings,
            SaveManager saveManager,
            PlacementManager placementMangaer,
            PlacedDominoManager placedDominoManager)
        {
            _settings = settings;
            _saveManager = saveManager;
            _placementManager = placementMangaer;
            _placedDominoManager = placedDominoManager;
            _settings.PlayBtn.onClick.AddListener(() => { PlayControl(!_isPlaying); });
        }

        public void Initialize()
        {
            Application.targetFrameRate = -1; // Set to target default framerate
            PlayControl(false);
        }

        private void PlayControl(bool state)
        {
            _isPlaying = state;
            if (state) _saveManager.Save("auto"); // Autosave when in play
            _placementManager.DestroyGhost();
            //_settings.HandObject.SetActive(state);
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
            public Button PlayBtn;
            public float Gravity = -50;
            public GameObject HandObject;
        }
    }
}
