using System;
using UnityEngine;

namespace Assets
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject TheCamera;
        public float MinHeight;
        public float MaxHeight;
        public float MaxMinX;
        public float MaxMinZ;
        public float FlySpeed = 0.5f;

        public float DragSpeed = 2;
        private float Height;
        private Vector3 _dragOrigin;
        private bool _shift;
        private bool _ctrl;
        
        private float _accelerationAmount = 1.5f;
        private readonly float _accelerationRatio = 3;
        private readonly float _slowDownRatio = 0.2f;

        [HideInInspector]
        public static CameraManager instance = null;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            MouseLook.instance.SetActive(Input.GetMouseButton(1));

            ////

            if (Input.GetMouseButton(2))
            {
                if (Input.GetMouseButtonDown(2))
                {
                    Height = TheCamera.transform.position.y;
                    _dragOrigin = Input.mousePosition;
                    Debug.Log("fired");
                }

                var pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - _dragOrigin);
                var move = new Vector3(pos.x, 0, pos.y) * -1; //* FlySpeed * -1;

                //var vec = (new Vector3(TheCamera.transform.forward.x, 0, TheCamera.transform.forward.z) * move.x) +
                 //         (new Vector3(TheCamera.transform.right.x, 0, TheCamera.transform.right.z) * move.y) * FlySpeed;

                //TheCamera.transform.Translate(vec, Space.World);

                TheCamera.transform.Translate(move * FlySpeed, Space.Self);
                TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, Height, TheCamera.transform.position.z);
            }



            ////
            //use shift to speed up flight
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                _shift = true;
                FlySpeed *= _accelerationRatio;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                _shift = false;
                FlySpeed /= _accelerationRatio;
            }

            //use ctrl to slow up flight
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                _ctrl = true;
                FlySpeed *= _slowDownRatio;
            }

            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            {
                _ctrl = false;
                FlySpeed /= _slowDownRatio;
            }
            
            if (Math.Abs(Input.GetAxis("Vertical")) > 0.01)
            {
                TheCamera.transform.Translate(new Vector3(TheCamera.transform.forward.x,0,TheCamera.transform.forward.z) * Input.GetAxis("Vertical") * FlySpeed, Space.World);
            }

            if (Math.Abs(Input.GetAxis("Horizontal")) > 0.01)
            {
                TheCamera.transform.Translate(new Vector3(TheCamera.transform.right.x, 0, TheCamera.transform.right.z) * Input.GetAxis("Horizontal") * FlySpeed, Space.World);
            }

            var d = Input.GetAxis("Mouse ScrollWheel");
            if (Input.GetKey(KeyCode.E) || d < 0f)
            {
                TheCamera.transform.position = TheCamera.transform.position + Vector3.up * FlySpeed;
            }
            else if (Input.GetKey(KeyCode.Q) || d > 0f)
            {
                TheCamera.transform.position = TheCamera.transform.position + Vector3.down * FlySpeed;
            }

            Clamp();
            //if (Input.GetKeyDown(KeyCode.M))
            //    playerObject.transform.position = transform.position; 
        }

        private void Clamp()
        {
            if (TheCamera.transform.position.y < MinHeight)
            {
                TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, MinHeight, TheCamera.transform.position.z);
            }

            if (TheCamera.transform.position.y > MaxHeight)
            {
                TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, MaxHeight, TheCamera.transform.position.z);
            }

            if (TheCamera.transform.position.x > MaxMinX)
            {
                TheCamera.transform.position = new Vector3(MaxMinX, TheCamera.transform.position.y, TheCamera.transform.position.z);
            }

            if (TheCamera.transform.position.x < -MaxMinX)
            {
                TheCamera.transform.position = new Vector3(-MaxMinX, TheCamera.transform.position.y, TheCamera.transform.position.z);
            }

            if (TheCamera.transform.position.z > MaxMinZ)
            {
                TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, TheCamera.transform.position.y, MaxMinZ);
            }

            if (TheCamera.transform.position.z < -MaxMinZ)
            {
                TheCamera.transform.position = new Vector3(TheCamera.transform.position.x, TheCamera.transform.position.y, -MaxMinZ);
            }
        }
    }
}
