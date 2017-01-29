using Assets.Scripts.Factories;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Controllers
{
    public class GhostController : ITickable
    {
        public GhostController(
            PrefabFactory prefabFactory)
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


        private readonly PrefabFactory _prefabFactory;
    }
}
