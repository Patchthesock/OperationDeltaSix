using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        public Button PlayBtn;
        public Button CreditBtn;
        public float CreditFadeTime;
        public GameObject HandObject;
        public GameObject Credits;

        public bool IsPlaying { get; private set; }

        private void InitGame()
        {
            if (PlayBtn != null)
            {
                PlayBtn.onClick.AddListener(() =>
                {
                    PlayControl(!IsPlaying);
                    
                });
                CreditBtn.onClick.AddListener(() =>
                {
                    ToggleCredit(_showCredit = !_showCredit);
                });
            }
            PlayControl(false);
            ToggleCredit(_showCredit);
        }

        public void PlayControl(bool state)
        {
            if(state) SaveManager.instance.Save();
            Physics.gravity = state ? new Vector3(0, -50, 0) : new Vector3(0, 0, 0);
            HandObject.SetActive(state);
            IsPlaying = state;
            PlayBtn.GetComponentInChildren<Text>().text = IsPlaying ? "Stop" : "Start";

            if (PlacementManager.instance == null) return;
            PlacementManager.instance.SetActive(!state);
            if (PlacedDominoManager.instance == null) return;
            PlacedDominoManager.instance.UpdatePlacedDominoPhysics(state);
        }

        private void ToggleCredit(bool state)
        {
            Credits.SetActive(state);
        }

        private bool _showCredit = false;

        [HideInInspector]
        public static GameManager instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            InitGame();
        }
    }
}
