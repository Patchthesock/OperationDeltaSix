using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        public Button PlayBtn;

        [HideInInspector]
        public static GameManager instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
            _isPlaying = false;
            InitGame();
        }

        private void InitGame()
        {
            // Play Button Control
            if (PlayBtn != null)
            {
                PlayBtn.onClick.AddListener(() =>
                {
                    PlayControl(!_isPlaying);
                });
            }
        }

        private void Update()
        {
            Debug.Log(_isPlaying);
        }

        private void PlayControl(bool state)
        {
            Physics.gravity = state ? new Vector3(0, -50, 0) : new Vector3(0, 0, 0);
            _isPlaying = !_isPlaying;
        }

        private bool _isPlaying;
    }
}
