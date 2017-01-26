using System.Collections.Generic;
using Assets.Scripts.Actors;
using Assets.Scripts.Factories;

namespace Assets.Scripts.Controllers
{
    public class DominoController
    {
        public DominoController(
            PrefabFactory prefabFactory)
        {
            _prefabFactory = prefabFactory;
        }



        private readonly PrefabFactory _prefabFactory;
        private readonly List<Domino> _activeDominos = new List<Domino>();
        private readonly List<Domino> _nonActiveDominos = new List<Domino>();
    }
}
