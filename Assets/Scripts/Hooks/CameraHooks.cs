using UnityEngine;

namespace Assets.Scripts.Actors
{
    public class CameraHooks : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public Camera Camera
        {
            get { return _camera; }
        }
    }
}
