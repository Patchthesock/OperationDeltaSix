using System;
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
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        public void Tick()
        {
            if (_isActive) RemoveItem();
        }

        private void RemoveItem()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            if (hit.collider == null) return;
            if (!_settings.AudioSource.isPlaying) _settings.AudioSource.Play();

            switch (hit.collider.gameObject.tag)
            {
                case "Ground":
                    return;
                case "Domino":
                    _placedDominoManager.RemoveDomino(hit.collider.gameObject);
                    return;
                case "Object":
                    _placedObjectManager.RemoveObject(hit.collider.gameObject);
                    return;
                default:
                    Debug.Log("RemovalManager.RemoveItem() unknown gameObject tag");
                    return;
            }
        }

        private bool _isActive;
        private readonly Settings _settings;
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
