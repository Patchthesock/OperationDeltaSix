using System;
using Assets.Scripts.Actors;
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
            InstallCameraController();
            InstallDominoController();
            InstallMenuController();
            InstallRemovalController();
            InstallGhostController();
            InstallInteractController();
            InstallPlacementController();
            InstallGameStateController();
            InstallGameController();
        }

        private void InstallInteractController()
        {
            Container.Bind<InteractController.Settings>()
                .FromInstance(_settings.InteractControllerSettings)
                .AsSingle();
            Container.Bind<ITickable>().To<InteractController>().AsSingle();
            Container.Bind<InteractController>().AsSingle();
        }

        private void InstallGameStateController()
        {
            Container.Bind<GameStateController.Settings>()
                .FromInstance(_settings.GameStateControllerSettings)
                .AsSingle();
            Container.Bind<GameStateController>().AsSingle();
        }

        private void InstallGameController()
        {
            Container.Bind<GameController>().AsSingle();
        }

        private void InstallInputController()
        {
            Container.Bind<InputController>().AsSingle();
        }

        private void InstallCameraController()
        {
            Container.Bind<ITickable>().To<CameraController>().AsSingle();
            Container.Bind<CameraController.Settings>()
                .FromInstance(_settings.CameraControllerSettings);
            Container.Bind<CameraController>().AsSingle();
        }

        private void InstallDominoController()
        {
            Container.Bind<Domino.Settings>()
                .FromInstance(_settings.DominoSettings)
                .AsSingle();
            Container.Bind<DominoController.Settings>()
                .FromInstance(_settings.DominoControllerSettings).AsSingle();
            Container.Bind<DominoController>().AsSingle();
        }

        private void InstallMenuController()
        {
            Container.Bind<MenuController.Settings>()
                .FromInstance(_settings.MenuControllerSettings).AsSingle();
            Container.Bind<MenuController>().AsSingle();
        }

        private void InstallRemovalController()
        {
            Container.Bind<RemovalController>().AsSingle();
        }

        private void InstallGhostController()
        {
            Container.Bind<GhostController.Settings>()
                .FromInstance(_settings.GhostControllerSettings).AsSingle();
            Container.Bind<ITickable>().To<GhostController>().AsSingle();
            Container.Bind<GhostController>().AsSingle();
        }

        private void InstallPlacementController()
        {
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
            public Domino.Settings DominoSettings = null;
            public MenuController.Settings MenuControllerSettings = null;
            public GhostController.Settings GhostControllerSettings = null;
            public CameraController.Settings CameraControllerSettings = null;
            public DominoController.Settings DominoControllerSettings = null;
            public InteractController.Settings InteractControllerSettings = null;
            public GameStateController.Settings GameStateControllerSettings = null;
        }
    }
}
