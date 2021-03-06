﻿using Assets.Scripts.Interfaces;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Components.GameModels
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]
    public class Domino : MonoBehaviour, IPlacementable
    {
        private AudioSource _audioSource;

        public GameObject GetGameObject()
        {
            return gameObject;
        }

        public void SetMass(float mass)
        {
            gameObject.GetComponent<Rigidbody>().mass = mass;
        }

        public void SetAudioManager(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag != "Domino") return;
            _audioSource.clip = _audioManager.GetDominoCollisionSound();
            _audioSource.Play();
        }

        private AudioManager _audioManager;
    }
}