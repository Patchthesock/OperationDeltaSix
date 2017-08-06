using System;
using UnityEngine;

namespace Assets.Scripts.Gui
{
    public class StateDisplayGuiManager
    {
        public StateDisplayGuiManager(Settings settings)
        {
            _settings = settings;
        }

        public void SetState(State state)
        {
            switch (state)
            {
                case State.Play:
                    Play();
                    break;
                
                case State.Edit:
                    Edit();
                    break;
                case State.Remove:
                    Remove();
                    break;
                case State.Introduction:
                    Introduction();
                    break;
                default:
                    Debug.LogError("StateDisplayManager: Invalid state: " + state);
                    break;
            }
        }

        public enum State
        {
            Play,
            Edit,
            Remove,
            Introduction
        }

        private void Play()
        {
            ClearAll();
            _settings.PlayState.SetActive(true);
        }

        private void Edit()
        {
            ClearAll();
            _settings.EditState.SetActive(true);
        }

        private void Remove()
        {
            ClearAll();
            _settings.RemvoveState.SetActive(true);
        }

        private void Introduction()
        {
            _settings.IntroductionState.SetActive(true);
        }

        private void ClearAll()
        {
            _settings.PlayState.SetActive(false);
            _settings.EditState.SetActive(false);
            _settings.RemvoveState.SetActive(false);
            _settings.IntroductionState.SetActive(false);
        }

        private readonly Settings _settings;

        [Serializable]
        public class Settings
        {
            public GameObject PlayState;
            public GameObject EditState;
            public GameObject RemvoveState;
            public GameObject IntroductionState;
        }
    }
}
