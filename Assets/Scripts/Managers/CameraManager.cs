using System;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject TheCamera;
        public float MinHeight;
        public float MaxHeight;
        public float MaxMinX;
        public float MaxMinZ;
        public float FlySpeed = 0.5f;

        private float _height;
        private Vector3 _dragOrigin;
        
        private float _accelerationAmount = 1.5f;
        private readonly float _accelerationRatio = 3;
        private readonly float _slowDownRatio = 0.2f;

        [HideInInspector]
        public static CameraManager Instance;

        private void Update()
        {
            MouseLook.instance.SetActive(Input.GetMouseButton(1));

            if (Input.GetMouseButton(2))
            {
                if (Input.GetMouseButtonDown(2))
                {
                    _height = TheCamera.transform.position.y;
                    _dragOrigin = Input.mousePosition;
                }

                var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
                TheCamera.transform.Translate(new Vector3(pos.x, 0, pos.y) * -1 * FlySpeed, Space.Self);
                TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, _height, TheCamera.transform.position.z);
            }

            if (Math.Abs(Input.GetAxis("Vertical")) > 0.01) TheCamera.transform.Translate(new Vector3(TheCamera.transform.forward.x,0,TheCamera.transform.forward.z) * Input.GetAxis("Vertical") * FlySpeed, Space.World);
            if (Math.Abs(Input.GetAxis("Horizontal")) > 0.01) TheCamera.transform.Translate(new Vector3(TheCamera.transform.right.x, 0, TheCamera.transform.right.z) * Input.GetAxis("Horizontal") * FlySpeed, Space.World);
            if (Input.GetKey(KeyCode.E) || Input.GetAxis("Mouse ScrollWheel") < 0f) TheCamera.transform.position = TheCamera.transform.position + Vector3.up * FlySpeed;
            else if (Input.GetKey(KeyCode.Q) || Input.GetAxis("Mouse ScrollWheel") > 0f) TheCamera.transform.position = TheCamera.transform.position + Vector3.down * FlySpeed;
            Clamp();
        }

        private void Clamp()
        {
            if (TheCamera.transform.position.x > MaxMinX) TheCamera.transform.position = new Vector3(MaxMinX, TheCamera.transform.position.y, TheCamera.transform.position.z);
            if (TheCamera.transform.position.z > MaxMinZ) TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, TheCamera.transform.position.y, MaxMinZ);
            if (TheCamera.transform.position.x < -MaxMinX) TheCamera.transform.position = new Vector3(-MaxMinX, TheCamera.transform.position.y, TheCamera.transform.position.z);
            if (TheCamera.transform.position.z < -MaxMinZ) TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, TheCamera.transform.position.y, -MaxMinZ);
            if (TheCamera.transform.position.y < MinHeight) TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, MinHeight, TheCamera.transform.position.z);
            if (TheCamera.transform.position.y > MaxHeight) TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, MaxHeight, TheCamera.transform.position.z);
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }
}
