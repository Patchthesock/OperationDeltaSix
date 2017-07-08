using System;
using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class Installer : MonoInstaller
    {
        public Settings GameSettings;

        public override void InstallBindings()
        {
            InstallPlacedManagers(Container, GameSettings.PlacedDominoManagerSettings);
            InstallSaveManager(Container, GameSettings.SaveManagerSettings);
            InstallRemovalManager(Container);
            InstallPlacementManager(Container, GameSettings.PlacementManagerSettings);
            InstallMenuManager(Container, GameSettings.MenuManagerSettings);
            InstallAudioManager(Container, GameSettings.AudioManagerSettings);
            InstallGameManager(Container, GameSettings.GameManagerSettings);
        }

        private void InstallPlacedManagers(DiContainer container, PlacedDominoManager.Settings settings)
        {
            container.Bind<PrefabFactory>().AsSingle();
            container.Bind<PlacedDominoManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<PlacedDominoManager>().AsSingle();
            container.Bind<PlacedObjectManager>().AsSingle();
        }

        private void InstallSaveManager(DiContainer container, SaveManager.Settings settings)
        {
            container.Bind<SaveManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<SaveManager>().AsSingle();
        }

        private void InstallRemovalManager(DiContainer container)
        {
            container.Bind<ITickable>().To<RemovalManager>().AsSingle();
            container.Bind<RemovalManager>().AsSingle();
        }

        private void InstallPlacementManager(DiContainer container, PlacementManager.Settings settings)
        {
            container.Bind<PlacementManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<PlacementManager>().AsSingle();
            container.Bind<ITickable>().To<PlacementManager>().AsSingle();
        }

        private void InstallMenuManager(DiContainer container, MenuManager.Settings settings)
        {
            container.Bind<MenuManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<MenuManager>().AsSingle();
            container.Bind<IInitializable>().To<MenuManager>().AsSingle();
        }

        private void InstallAudioManager(DiContainer container, AudioManager.Settings settings)
        {
            container.Bind<AudioManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<AudioManager>().AsSingle();
        }

        private void InstallGameManager(DiContainer container, GameManager.Settings settings)
        {
            container.Bind<GameManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<GameManager>().AsSingle();
            container.Bind<IInitializable>().To<GameManager>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public MenuManager.Settings MenuManagerSettings;
            public SaveManager.Settings SaveManagerSettings;
            public GameManager.Settings GameManagerSettings;
            public AudioManager.Settings AudioManagerSettings;
            public PlacementManager.Settings PlacementManagerSettings;
            public PlacedDominoManager.Settings PlacedDominoManagerSettings;
        }
    }
}
