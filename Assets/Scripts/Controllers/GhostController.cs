using Assets.Scripts.Factories;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Controllers
{
    public class GhostController : ITickable
    {
        public GhostController(
            PrefabFactory prefabFactory,
            InputController inputController)
        {
            _prefabFactory = prefabFactory;

        }

        public void Select(GameObject model)
        {
            
        }

        public void Drop()
        {
            
        }

        public void Tick()
        {
            
        }

        private GameObject _objectToPlace;
        private readonly PrefabFactory _prefabFactory;
        private readonly InputController _inputController;
    }
}
