using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Components;
using Assets.Scripts.Components.GameModels;
using Assets.Scripts.Managers.Models;
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
            foreach (var o in _placedDominos)
            {
                o.Domino.GetComponentInChildren<Rigidbody>().useGravity = usePhysics;
                o.Domino.GetComponentInChildren<Rigidbody>().isKinematic = !usePhysics;
            }
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
            // TODO
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
            model.GetComponentInChildren<Rigidbody>().isKinematic = true;
            model.GetComponentInChildren<Rigidbody>().useGravity = false;
            model.transform.position = position;
            model.transform.rotation = rotation;
            _placedDominos.Add(new LocalDomino(model));
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
                Save = new SaveObject
                {
                    Name = domino.name,
                    Position = domino.transform.position,
                    Rotation = domino.transform.rotation
                };

                Domino = domino;
            }

            public readonly SaveObject Save;
            public readonly GameObject Domino;
        }
    }
}
