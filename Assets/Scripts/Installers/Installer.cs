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
            Container.Bind<PrefabFactory>().AsSingle();
            Container.Bind<PlacedDominoManager.Settings>()
                .FromInstance(GameSettings.PlacedDominoManagerSettings)
                .AsSingle();
            Container.Bind<PlacedDominoManager>().AsSingle();
            Container.Bind<PlacedObjectManager>().AsSingle();

            Container.Bind<SaveManager.Settings>()
                .FromInstance(GameSettings.SaveManagerSettings)
                .AsSingle();

            Container.Bind<SaveManager>().AsSingle();

            Container.Bind<PlacementManager.Settings>()
                .FromInstance(GameSettings.PlacementManagerSettings)
                .AsSingle();

            Container.Bind<PlacementManager>().AsSingle();
            Container.Bind<ITickable>().To<PlacementManager>().AsSingle();

            Container.Bind<MenuManager.Settings>()
                .FromInstance(GameSettings.MenuManagerSettings)
                .AsSingle();

            Container.Bind<MenuManager>().AsSingle();
            Container.Bind<IInitializable>().To<MenuManager>().AsSingle();

            Container.Bind<GameManager.Settings>()
                .FromInstance(GameSettings.GameManagerSettings)
                .AsSingle();
            Container.Bind<GameManager>().AsSingle();
            Container.Bind<IInitializable>().To<GameManager>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public MenuManager.Settings MenuManagerSettings;
            public SaveManager.Settings SaveManagerSettings;
            public GameManager.Settings GameManagerSettings;
            public PlacementManager.Settings PlacementManagerSettings;
            public PlacedDominoManager.Settings PlacedDominoManagerSettings;
        }
    }
}
