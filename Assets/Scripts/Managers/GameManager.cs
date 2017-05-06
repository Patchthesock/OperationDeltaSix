using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public Button PlayBtn;
        public GameObject HandObject;
        public bool IsPlaying { get; private set; }

        private void InitGame()
        {
            PlayControl(false);
            PlayBtn.onClick.AddListener(() => { PlayControl(!IsPlaying); });
        }

        public void PlayControl(bool state)
        {
            if(state) SaveManager.instance.Save();
            Physics.gravity = state ? new Vector3(0, -50, 0) : new Vector3(0, 0, 0);
            HandObject.SetActive(state);
            IsPlaying = state;

            if (state) Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            if (PlacementManager.Instance == null) return;
            PlacementManager.Instance.SetActive(!state);
            if (PlacedDominoManager.Instance == null) return;
            PlacedDominoManager.Instance.SetDominoPhysics(state);
        }

        private bool _showCredit = false;

        [HideInInspector]
        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            InitGame();
        }
    }
}
