using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        public Button PlayBtn;
        public GameObject HandObject;

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
            PlayControl(false);
        }

        public void PlayControl(bool state)
        {
            Physics.gravity = state ? new Vector3(0, -50, 0) : new Vector3(0, 0, 0);
            PlacedObjectManager.instance.UpdatePlacedObjectPhysics(state);
            HandObject.SetActive(state);
            IsPlaying = state;
        }
    }
}
