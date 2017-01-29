using System;
using Assets.Scripts.Controllers;
using Assets.Scripts.Factories;
using Assets.Scripts.Services;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private Settings _settings = null;

        public override void InstallBindings()
        {
            InstallServices();
            InstallFactories();
            InstallControllers();
            InstallAppController();
        }

        #region Factories
        private void InstallFactories()
        {
            InstallPrefabFactory();
            InstallGameStateFactory();
        }

        private void InstallPrefabFactory()
        {
            Container.Bind<PrefabFactory>().AsSingle();
        }

        private void InstallGameStateFactory()
        {
            Container.Bind<GameStateFactory>().AsSingle();
        }
        #endregion

        #region Services
        private void InstallServices()
        {
            InstallLoadSaveService();
            InstallStandaloneInputService();
        }

        private void InstallLoadSaveService()
        {
            Container.Bind<LoadSaveService>().AsSingle();
        }

        private void InstallStandaloneInputService()
        {
            Container.Bind<StandaloneInputService>().AsSingle();
        }
        #endregion

        #region Controllers
        private void InstallControllers()
        {
            InstallInputController();
            InstallLevelController();
            InstallCameraController();
            InstallDominoController();
            InstallPlacementController();
            InstallGameController();
        }

        private void InstallGameController()
        {
            Container.Bind<GameController>().AsSingle();
        }

        private void InstallInputController()
        {
            Container.Bind<InputController>().AsSingle();
        }

        private void InstallLevelController()
        {
            Container.Bind<LevelController.Settings>()
                .FromInstance(_settings.LevelControllerSettings).AsSingle();
            Container.Bind<LevelController>().AsSingle();
        }

        private void InstallCameraController()
        {
            Container.Bind<CameraController.Settings>()
                .FromInstance(_settings.CameraControllerSettings);
            Container.Bind<CameraController>().AsSingle();
        }

        private void InstallDominoController()
        {
            Container.Bind<DominoController.Settings>()
                .FromInstance(_settings.DominoControllerSettings).AsSingle();
            Container.Bind<DominoController>().AsSingle();
        }

        private void InstallPlacementController()
        {
            Container.Bind<PlacementController.Settings>()
                .FromInstance(_settings.PlacementControllerSettings).AsSingle();
            Container.Bind<PlacementController>().AsSingle();
        }
        #endregion

        private void InstallAppController()
        {
            Container.Bind<IInitializable>().To<AppController>().AsSingle();
            Container.Bind<AppController>().AsSingle();
        }

        [Serializable]
        private class Settings
        {
            public LevelController.Settings LevelControllerSettings;
            public CameraController.Settings CameraControllerSettings;
            public DominoController.Settings DominoControllerSettings;
            public PlacementController.Settings PlacementControllerSettings;
        }
    }
}
