using Assets.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        public Button PlayBtn;
        public GameObject HandObject;
        public Color StartBtnColorActive;
        public Color StartBtnColorNotActive;

        public bool IsPlaying { get; private set; }

        private void InitGame()
        {
            if (PlayBtn != null)
            {
                PlayBtn.onClick.AddListener(() =>
                {
                    PlayControl(!IsPlaying);
                });
            }
            PlayControl(false);
            PlayBtn.GetComponent<Image>().color = StartBtnColorActive;
        }

        public void PlayControl(bool state)
        {
            if(state) SaveManager.instance.Save();
            Physics.gravity = state ? new Vector3(0, -50, 0) : new Vector3(0, 0, 0);
            HandObject.SetActive(state);
            IsPlaying = state;
            // PlayBtn.GetComponentInChildren<Text>().text = IsPlaying ? "Stop" : "Begin";
            PlayBtn.GetComponent<Image>().color = IsPlaying ? StartBtnColorNotActive : StartBtnColorActive;

            if (state) Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            if (PlacementManager.instance == null) return;
            PlacementManager.instance.SetActive(!state);
            if (PlacedDominoManager.instance == null) return;
            PlacedDominoManager.instance.UpdatePlacedDominoPhysics(state);
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
