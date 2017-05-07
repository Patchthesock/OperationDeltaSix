using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Components.GameModels
{
    [RequireComponent(typeof(AudioSource))]
    public class Domino : MonoBehaviour, IPlacementable
    {
        public AudioClip[] audioFiles;
        private AudioSource _audioSource;

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag != "Domino") return;
            _audioSource.clip = audioFiles[Random.Range(0, audioFiles.Length)];
            _audioSource.Play();
        }
    }

    public class Dominos : MonoBehaviour, IPlacementable
    {
        public List<Domino> Domino;

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}