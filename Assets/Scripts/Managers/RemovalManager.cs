using System;
using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Managers
{
    public class RemovalManager : ITickable
    {
        public RemovalManager(
            Settings settings,
            PlacedDominoManager placedDominoManager,
            PlacedDominoPropManager placedObjectManager)
        {
            _isActive = false;
            _settings = settings;
            _placedDominoManager = placedDominoManager;
            _placedObjectManager = placedObjectManager;
            _settings.AudioSource.clip = _settings.RemovalClip;
            _typeDict = Functions.GetPlaceableTypeDictionary();
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        public void Tick()
        {
            if (!_isActive) return;
            if (Functions.GetMouseButtonDownInput(0)) RemoveItem();
        }

        private void RemoveItem()
        {
            var hit = Functions.GetHit();
            if (hit.collider == null) return;
            var m = hit.collider.gameObject.GetComponent<IPlacementable>() ??
                    hit.collider.gameObject.transform.parent.gameObject.GetComponent<IPlacementable>();
            if (m == null) return;
            if (!_settings.AudioSource.isPlaying) _settings.AudioSource.Play();

            switch (_typeDict[m.GetType()])
            {
                case 0:
                    _placedDominoManager.RemoveDomino(m.GetGameObject());
                    return;
                case 1:
                    return;
                case 2:
                    _placedObjectManager.RemoveObject(m.GetGameObject());
                    return;
                default:
                    return;
            }
        }

        private bool _isActive;
        private readonly Settings _settings;
        private readonly Dictionary<Type, int> _typeDict;
        private readonly PlacedDominoManager _placedDominoManager;
        private readonly PlacedDominoPropManager _placedObjectManager;

        [Serializable]
        public class Settings
        {
            public AudioClip RemovalClip;
            public AudioSource AudioSource;
        }
    }
}
