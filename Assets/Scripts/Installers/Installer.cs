using System;
using Assets.Scripts.Components;
using Assets.Scripts.Gui;
using Assets.Scripts.Gui.Components;
using Assets.Scripts.Gui.Services;
using Assets.Scripts.Managers;
using Assets.Scripts.Service;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class Installer : MonoInstaller
    {
        public Settings GameSettings;

        public override void InstallBindings()
        {
            InstallStateDisplayManager(Container, GameSettings.GuiComponentPrefabs.StateDisplayGui);
            InstallPlacedManagers(Container, GameSettings.PlacedDominoManagerSettings, GameSettings.PlacedDominoPropManagerSettings);
            InstallLocalFilePersistance(Container);
            InstallSaveManager(Container);
            InstallBtnOptionFactory(Container, GameSettings.BtnOptionSettings);
            InstallSaveGuiManager(Container, GameSettings.GuiComponentPrefabs.SaveGui, GameSettings.GuiComponentPrefabs.SaveConfirmGui);
            InstallLoadGuiManager(Container, GameSettings.GuiComponentPrefabs.LoadGui);
            InstallRemovalManager(Container, GameSettings.RemovalManagerSettings);
            InstallCameraManager(Container, GameSettings.CameraManagerSettings);
            InstallPlacementManager(Container, GameSettings.PlacementManagerSettings);
            InstallMenuManager(Container, GameSettings.MenuManagerSettings, GameSettings.GuiComponentPrefabs.MainMenuGui);
            InstallAudioManager(Container, GameSettings.AudioManagerSettings);
            InstallDominoInteractionManager(Container, GameSettings.InteractionSettings);
            InstallGameManager(Container, GameSettings.GameManagerSettings);
        }

        private static void InstallStateDisplayManager(DiContainer container, StateDisplayGuiManager.Settings settings)
        {
            container.Bind<StateDisplayGuiManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<StateDisplayGuiManager>().AsSingle();
        }

        private static void InstallPlacedManagers(DiContainer container, PlacedDominoManager.Settings settings, PlacedDominoPropManager.Settings propSettings)
        {
            container.Bind<PrefabFactory>().AsSingle();
            container.Bind<PlacedDominoManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<PlacedDominoManager>().AsSingle();
            container.Bind<PlacedDominoPropManager.Settings>().FromInstance(propSettings).AsSingle();
            container.Bind<PlacedDominoPropManager>().AsSingle();
        }

        private static void InstallLocalFilePersistance(DiContainer container)
        {
            container.Bind<LocalFilePersistance>().AsSingle();
        }

        private static void InstallSaveManager(DiContainer container)
        {
            container.Bind<SaveManager>().AsSingle();
        }

        private static void InstallBtnOptionFactory(DiContainer container, BtnOptionFactory.Settings settings)
        {
            container.Bind<BtnOptionFactory.Settings>().FromInstance(settings).AsSingle();
            container.Bind<BtnOptionFactory>().AsSingle();
        }

        private static void InstallSaveGuiManager(DiContainer container, SaveGui saveGui, SaveConfirmGui saveConfirmGui)
        {
            container.Bind<SaveGui>().FromInstance(saveGui).AsSingle();
            container.Bind<SaveConfirmGui>().FromInstance(saveConfirmGui).AsSingle();
            container.Bind<SaveGuiManager>().AsSingle();
        }

        private static void InstallLoadGuiManager(DiContainer container, LoadGui loadGui)
        {
            container.Bind<LoadGui>().FromInstance(loadGui).AsSingle();
            container.Bind<LoadGuiManager>().AsSingle();
        }

        private static void InstallRemovalManager(DiContainer container, RemovalManager.Settings settings)
        {
            container.Bind<RemovalManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<ITickable>().To<RemovalManager>().AsSingle();
            container.Bind<RemovalManager>().AsSingle();
        }

        private static void InstallCameraManager(DiContainer container, CameraManager.Settings settings)
        {
            container.Bind<CameraManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<CameraManager>().AsSingle();
            container.Bind<ITickable>().To<CameraManager>().AsSingle();
        }

        private static void InstallPlacementManager(DiContainer container, PlacementManager.Settings settings)
        {
            container.Bind<PlacementManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<PlacementManager>().AsSingle();
            container.Bind<ITickable>().To<PlacementManager>().AsSingle();
        }

        private static void InstallMenuManager(DiContainer container, MenuManager.Settings settings, MainMenuGui mainGui)
        {
            container.Bind<MainMenuGui>().FromInstance(mainGui).AsSingle();
            container.Bind<MenuManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<MenuManager>().AsSingle();
            container.Bind<ITickable>().To<MenuManager>().AsSingle();
        }

        private static void InstallAudioManager(DiContainer container, AudioManager.Settings settings)
        {
            container.Bind<AudioManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<AudioManager>().AsSingle();
        }

        private static void InstallDominoInteractionManager(DiContainer container, DominoInteractionManager.Settings settings)
        {
            container.Bind<DominoInteractionManager.Settings>().FromInstance(settings);
            container.Bind<DominoInteractionManager>().AsSingle();
            container.Bind<ITickable>().To<DominoInteractionManager>().AsSingle();
        }

        private static void InstallGameManager(DiContainer container, GameManager.Settings settings)
        {
            container.Bind<GameManager.Settings>().FromInstance(settings).AsSingle();
            container.Bind<GameManager>().AsSingle();
            container.Bind<IInitializable>().To<GameManager>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public GuiComponents GuiComponentPrefabs;
            public MenuManager.Settings MenuManagerSettings;
            public GameManager.Settings GameManagerSettings;
            public AudioManager.Settings AudioManagerSettings;
            public CameraManager.Settings CameraManagerSettings;
            public BtnOptionFactory.Settings BtnOptionSettings;
            public RemovalManager.Settings RemovalManagerSettings;
            public PlacementManager.Settings PlacementManagerSettings;
            public DominoInteractionManager.Settings InteractionSettings;
            public PlacedDominoManager.Settings PlacedDominoManagerSettings;
            public PlacedDominoPropManager.Settings PlacedDominoPropManagerSettings;

            [Serializable]
            public class GuiComponents
            {
                public SaveGui SaveGui;
                public LoadGui LoadGui;
                public MainMenuGui MainMenuGui;
                public SaveConfirmGui SaveConfirmGui;
                public StateDisplayGuiManager.Settings StateDisplayGui;
            }
        }
    }
}
