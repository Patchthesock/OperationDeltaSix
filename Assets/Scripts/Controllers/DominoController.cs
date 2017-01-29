using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using Assets.Scripts.Factories;
using Assets.Scripts.Hooks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Controllers
{
    public class DominoController
    {
        public DominoController(
            Settings settings,
            PrefabFactory prefabFactory)
        {
            _settings = settings;
            _prefabFactory = prefabFactory;
        }

        private Domino GetDomino()
        {
            return _nonActiveDominos.Count > 0 ? _nonActiveDominos.First() : CreateDomino();
        }

        private Domino CreateDomino()
        {
            return new Domino(
                _prefabFactory.Create(
                    _settings.Dominos[Random.Range(
                        0, _settings.Dominos.Count - 1)]
                        ).GetComponent<DominoHooks>());
        }

        private void RemoveDomino()
        {
            
        }

        private readonly Settings _settings;
        private readonly PrefabFactory _prefabFactory;
        private readonly List<Domino> _activeDominos = new List<Domino>();
        private readonly List<Domino> _nonActiveDominos = new List<Domino>();

        [Serializable]
        public class Settings
        {
            public List<GameObject> Dominos;
        }
    }
}
