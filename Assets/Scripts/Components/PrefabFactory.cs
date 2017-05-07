using UnityEngine;
using Zenject;

namespace Assets.Scripts.Components
{
    public class PrefabFactory
    {
        public PrefabFactory(DiContainer container)
        {
            _container = container;
        }

        public GameObject Instantiate(GameObject model, params object[] constructorArgs)
        {
            return _container.InstantiatePrefab(model);
        }

        private readonly DiContainer _container;
    }
}
