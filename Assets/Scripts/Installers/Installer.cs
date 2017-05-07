using Assets.Scripts.Components;
using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class Installer : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Menu>().AsSingle();
            Container.Bind<PrefabFactory>().AsSingle();
            Container.Bind<PlacedDominoManager.Settings>().AsSingle();
            Container.Bind<PlacedDominoManager>().AsSingle();
            Container.Bind<PlacedObjectManager>().AsSingle();
            Container.Bind<SaveManager>().AsSingle();
            Container.Bind<PlacementManager.Settings>().AsSingle();
            Container.Bind<PlacementManager>().AsSingle();
            Container.Bind<ITickable>().To<PlacementManager>().AsSingle();
            Container.Bind<GameManager.Settings>().AsSingle();
            Container.Bind<GameManager>().AsSingle();
            Container.Bind<IInitializable>().To<GameManager>().AsSingle();
        }
    }
}
