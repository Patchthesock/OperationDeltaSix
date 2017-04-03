using System;
using System.Collections.Generic;
using Assets.Scripts.Hooks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Actors
{
    public class Domino
    {
        public Domino(
            string name,
            Settings settings,
            DominoHooks hooks)
        {
            _hooks = hooks;
            _settings = settings;
            _hooks.gameObject.name = name;
            _hooks.OnCollision += OnCollisionEnter;
        }

        public string Name
        {
            get { return _hooks.gameObject.name; } 
        }

        public Transform Transform
        {
            get { return _hooks.Transform; }
        }

        public void SetPhysics(bool state)
        {
            _hooks.gameObject.layer = state ? 1 : 2;
            if (_hooks.Collider != null)
            {
                _hooks.Collider.isTrigger = !state;
            }
            if (_hooks.Rigidbody == null) return;
            _hooks.Rigidbody.useGravity = state;
            _hooks.Rigidbody.isKinematic = !state;
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag != "Domino") return;
            PlayAudio();
        }

        private void PlayAudio()
        {
            _hooks.AudioSource.clip = _settings.AudioClips[Random.Range(0, _settings.AudioClips.Count)];
            _hooks.AudioSource.Play();
        }

        private readonly Settings _settings;
        private readonly DominoHooks _hooks;

        [Serializable]
        public class Settings
        {
            public List<AudioClip> AudioClips = new List<AudioClip>();
        }
    }
}
