using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Components
{
    class DominoHooks : MonoBehaviour
    {
        public List<GameObject> Dominos;

        public AudioClip[] audioFiles;
        private AudioSource _audioSource;

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }


        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag != "Domino") return;
            _audioSource.clip = audioFiles[Random.Range(0, audioFiles.Length)];
            _audioSource.Play();
        }
    }
}
