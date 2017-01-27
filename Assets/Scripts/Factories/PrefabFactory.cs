using UnityEngine;
using Zenject;

namespace Assets.Scripts.Factories
{
    public class PrefabFactory
    {
        public PrefabFactory(DiContainer container)
        {
            _container = container;
        }

        public GameObject Create(GameObject prefab, params object[] constructorArgs)
        {
            return _container.InstantiatePrefab(prefab, constructorArgs);
        }

        private readonly DiContainer _container;
    }
}
