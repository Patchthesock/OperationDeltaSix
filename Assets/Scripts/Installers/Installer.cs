using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;
using Zenject;

public class Installer : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlacedDominoManager.Settings>().AsSingle();
        Container.Bind<PlacedDominoManager>().AsSingle();
        Container.Bind<PlacedObjectManager>().AsSingle();
        Container.Bind<SaveManager>().AsSingle();
        Container.Bind<PlacementManager.Settings>().AsSingle();
        Container.Bind<PlacementManager>().AsSingle();
        Container.Bind<GameManager.Settings>().AsSingle();
        Container.Bind<GameManager>().AsSingle();
    }


}
