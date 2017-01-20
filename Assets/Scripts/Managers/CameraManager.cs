using UnityEngine;

namespace Assets
{
    public class CameraManager : MonoBehaviour
    {
        [HideInInspector]
        public static CameraManager instance = null;

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

        }

        private void Update()
        {

        }
    }
}
