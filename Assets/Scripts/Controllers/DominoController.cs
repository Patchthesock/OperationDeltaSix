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

        public bool CanPlanDomino(Vector3 position)
        {
            return CanPlace(_settings.MinimumDistance, position, _activeDominos.Select(i => i.Transform.position));
        }

        public void PlaceDomino(string name, Vector3 position, Vector3 rotation)
        {
            if (!CanPlace(_settings.MinimumDistance, position, _activeDominos.Select(i => i.Transform.position)))
                return;

            var domino = GetDomino(name);
            domino.SetPhysics(false);
            _activeDominos.Add(domino);
            domino.Transform.position = position;
            domino.Transform.rotation = Quaternion.Euler(rotation);
        }

        public void RemoveDomino(string name)
        {
            var domino = _activeDominos.SingleOrDefault(t => t.Name == name);
            if (domino == null) return;
            if (!_activeDominos.Contains(domino)) return;
            _activeDominos.Remove(domino);
            _nonActiveDominos.Add(domino);
            domino.Transform.position = new Vector3(999, 999, 999);
        }

        public void SetPhysics(bool state)
        {
            foreach (var d in _activeDominos)
            {
                d.SetPhysics(state);
            }
        }

        private Domino GetDomino(string name)
        {
            return _nonActiveDominos.Count > 0 ? _nonActiveDominos.First() : CreateDomino(name);
        }

        private Domino CreateDomino(string name)
        {
            return new Domino(
                name,
                _prefabFactory.Create(
                    _settings.Dominos[Random.Range(
                        0, _settings.Dominos.Count - 1)]
                        ).GetComponent<DominoHooks>());
        }

        private static bool CanPlace(float minimumDistance, Vector3 position, IEnumerable<Vector3> active)
        {
            return active.All(p => !(Vector3.Distance(p, position) <= minimumDistance));
        }

        private readonly Settings _settings;
        private readonly PrefabFactory _prefabFactory;
        private readonly List<Domino> _activeDominos = new List<Domino>();
        private readonly List<Domino> _nonActiveDominos = new List<Domino>();

        [Serializable]
        public class Settings
        {
            public float MinimumDistance;
            public List<GameObject> Dominos;
        }
    }
}
