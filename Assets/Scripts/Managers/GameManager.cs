using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        public Button PlayBtn;

        public bool IsPlaying { get; private set; }

        [HideInInspector]
        public static GameManager instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            IsPlaying = false;
            InitGame();
        }

        private void InitGame()
        {
            // Play Button Control
            if (PlayBtn != null)
            {
                PlayBtn.onClick.AddListener(() =>
                {
                    PlayControl(!IsPlaying);
                });
            }
        }

        private void Update()
        {
            Debug.Log(IsPlaying);
        }

        private void PlayControl(bool state)
        {
            Physics.gravity = state ? new Vector3(0, -50, 0) : new Vector3(0, 0, 0);
            IsPlaying = !IsPlaying;
        }
    }
}
