using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class PlacementManager : MonoBehaviour
    {
        [HideInInspector] public static PlacementManager instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (_gameManager == null)
            {
                _gameManager = GameManager.instance;
                return;
            }
            if (_gameManager.IsPlaying)
            {
                Debug.Log("I Can Place shit");
            }
        }

        private GameManager _gameManager;
    }
}
