using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class AudioManager
    {
        public AudioManager(Settings settings)
        {
            _settings = settings;
        }

        public AudioClip GetDominoCollisionSound()
        {
            return _settings.DominoCollision[Random.Range(0, _settings.DominoCollision.Count-1)];
        }

        private readonly Settings _settings;

        public class Settings
        {
            public List<AudioClip> DominoCollision;
        }
    }
}
