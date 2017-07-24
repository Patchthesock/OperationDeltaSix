using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components;
using Assets.Scripts.Components.GameModels;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlacedDominoManager
    {
        public PlacedDominoManager(
            Settings settings,
            AudioManager audioManager,
            PrefabFactory prefabFactory)
        {
            _settings = settings;
            _audioManager = audioManager;
            _prefabFactory = prefabFactory;
        }

        public void PlaceDomino(Vector3 position, Quaternion rotation)
        {
            if (_nonActiveDominos.Count <= 0) PlaceNewDomino(position, rotation);
            else PlaceExistingDomino(position, rotation);
        }

        public void PlaceDomino(IEnumerable<SaveObject> dominos)
        {
            RemoveDomino();
            foreach (var o in dominos) PlaceDomino(o.Position, o.Rotation);
        }

        public void SetPhysics(bool usePhysics)
        {
            foreach (var o in _placedDominos) SetPhysics(o.Domino.GetComponentInChildren<Rigidbody>(), usePhysics);
        }

        public IEnumerable<GameObject> GetPlacedDominos()
        {
            return _placedDominos.Select(i => i.Domino);
        }

        public IEnumerable<SaveObject> GetPlacedDominosSave()
        {
            return _placedDominos.Select(i => i.Save);
        }

        public void RemoveDomino()
        {
            foreach (var o in _placedDominos.ToList()) RemoveDomino(o);
        }

        public void RemoveDomino(GameObject o)
        {
            var d = _placedDominos.Single(i => i.Domino == o);
            if (d != null) RemoveDomino(d);
        }

        private void RemoveDomino(LocalDomino o)
        {
            _placedDominos.Remove(o);
            _nonActiveDominos.Add(o.Domino);
            o.Domino.SetActive(false);
        }

        private void PlaceNewDomino(Vector3 position, Quaternion rotation)
        {
            var d = _prefabFactory.Instantiate(Functions.PickRandomObject(_settings.Dominos));
            if (_settings.ParentContainer != null) d.transform.SetParent(_settings.ParentContainer.transform);
            d.GetComponent<Domino>().SetAudioManager(_audioManager);
            PlaceObject(d, position, rotation);
        }

        private void PlaceExistingDomino(Vector3 position, Quaternion rotation)
        {
            var o = _nonActiveDominos.First();
            _nonActiveDominos.Remove(o);
            o.SetActive(true);
            PlaceObject(o, position, rotation);
        }
        
        private void PlaceObject(GameObject model, Vector3 position, Quaternion rotation)
        {
            SetPhysics(model.GetComponentInChildren<Rigidbody>(), false);
            model.transform.position = position;
            model.transform.rotation = rotation;
            _placedDominos.Add(new LocalDomino(model));
        }

        private static void SetPhysics(Rigidbody r, bool state)
        {
            r.useGravity = state;
            r.isKinematic = !state;
        }

        private readonly Settings _settings;
        private readonly AudioManager _audioManager;
        private readonly PrefabFactory _prefabFactory;
        private readonly List<LocalDomino> _placedDominos = new List<LocalDomino>();
        private readonly List<GameObject> _nonActiveDominos = new List<GameObject>();

        [Serializable]
        public class Settings
        {
            public GameObject ParentContainer;
            public List<GameObject> Dominos;
        }

        private class LocalDomino
        {
            public LocalDomino(GameObject domino)
            {
                Domino = domino;
                Save = new SaveObject
                {
                    Name = domino.name,
                    Position = domino.transform.position,
                    Rotation = domino.transform.rotation
                };
            }

            public readonly SaveObject Save;
            public readonly GameObject Domino;
        }
    }
}
