using Assets.Scripts.Controllers;
using Assets.Scripts.Factories;
using Assets.Scripts.Services;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class Installer : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallServices();
            InstallFactories();
            InstallControllers();
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
            InstallAppController();
            InstallGameController();
            InstallInputController();
            InstallCameraController();
            InstallDominoController();
            InstallPlacementController();
        }

        private void InstallAppController()
        {
            Container.Bind<AppController>().AsSingle();
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
            Container.Bind<CameraController>().AsSingle();
        }

        private void InstallDominoController()
        {
            Container.Bind<DominoController>().AsSingle();
        }

        private void InstallPlacementController()
        {
            Container.Bind<PlacementController>().AsSingle();
        }
        #endregion
    }
}
