﻿using System;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class GameManager : IInitializable
    {
        public GameManager(
            Settings settings,
            MenuManager menuManager,
            SaveManager saveManager,
            CameraManager cameraManager,
            PlacementManager placementMangaer,
            PlacedDominoManager placedDominoManager,
            DominoInteractionManager dominoInteractionManager)
        {
            _settings = settings;
            _menuManager = menuManager;
            _saveManager = saveManager;
            _cameraManager = cameraManager;
            _placementManager = placementMangaer;
            _placedDominoManager = placedDominoManager;
            _dominoInteractionManager = dominoInteractionManager;
        }

        public void Initialize()
        {
            Application.targetFrameRate = _settings.TargetFrameRate;
            PlayControl(false);
            _cameraManager.SetActive(true);
            _menuManager.Initialize(PlayControl);
            _menuManager.SubToOnMenuToggle(OnMenuToggled);
        }

        private void PlayControl(bool state)
        {
            if (state) _saveManager.Save("auto"); // Autosave when in play
            _placementManager.DestroyGhost();
            _placedDominoManager.SetPhysics(state);
            _dominoInteractionManager.SetActive(state);
            Physics.gravity = state ? new Vector3(0, _settings.Gravity, 0) : Vector3.zero;
        }

        private void OnMenuToggled(bool state)
        {
            _cameraManager.SetActive(!state); // Turn off camera movement on menu active
        }

        private readonly Settings _settings;
        private readonly MenuManager _menuManager;
        private readonly SaveManager _saveManager;
        private readonly CameraManager _cameraManager;
        private readonly PlacementManager _placementManager;
        private readonly PlacedDominoManager _placedDominoManager;
        private readonly DominoInteractionManager _dominoInteractionManager;

        [Serializable]
        public class Settings
        {
            public float Gravity = -50;
            public int TargetFrameRate = -1; // -1 = Set to target default framerate
        }
    }
}
