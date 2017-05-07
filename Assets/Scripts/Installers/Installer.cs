using System;
using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class Installer : MonoInstaller
    {
        public Settings GameSettings;

        public override void InstallBindings()
        {
            Container.Bind<Menu>().FromInstance(GameSettings.Menu).AsSingle();
            Container.Bind<PrefabFactory>().AsSingle();
            Container.Bind<PlacedDominoManager.Settings>()
                .FromInstance(GameSettings.PlacedDominoManagerSettings)
                .AsSingle();
            Container.Bind<PlacedDominoManager>().AsSingle();
            Container.Bind<PlacedObjectManager>().AsSingle();
            Container.Bind<SaveManager>().AsSingle();
            Container.Bind<PlacementManager.Settings>()
                .FromInstance(GameSettings.PlacementManagerSettings)
                .AsSingle();
            Container.Bind<PlacementManager>().AsSingle();
            Container.Bind<ITickable>().To<PlacementManager>().AsSingle();
            Container.Bind<GameManager.Settings>()
                .FromInstance(GameSettings.GameManagerSettings)
                .AsSingle();
            Container.Bind<GameManager>().AsSingle();
            Container.Bind<IInitializable>().To<GameManager>().AsSingle();
        }

        [Serializable]
        public class Settings
        {
            public Menu Menu;
            public GameManager.Settings GameManagerSettings;
            public PlacementManager.Settings PlacementManagerSettings;
            public PlacedDominoManager.Settings PlacedDominoManagerSettings;
        }
    }
}
