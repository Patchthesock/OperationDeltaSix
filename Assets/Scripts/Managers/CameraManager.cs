using System;
using UnityEngine;

namespace Assets
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject Camera;
        public float FlySpeed = 0.5f;

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
                Camera.transform.Translate(new Vector3(Camera.transform.forward.x,0,Camera.transform.forward.z) * Input.GetAxis("Vertical") * FlySpeed, Space.World);
            }

            if (Math.Abs(Input.GetAxis("Horizontal")) > 0.01)
            {
                Camera.transform.Translate(new Vector3(Camera.transform.right.x, 0, Camera.transform.right.z) * Input.GetAxis("Horizontal") * FlySpeed, Space.World);
            }

            if (Input.GetKey(KeyCode.E))
            {
                Camera.transform.Translate(Vector3.up * FlySpeed);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                Camera.transform.Translate(Vector3.down * FlySpeed);
            }


            //if (Input.GetKeyDown(KeyCode.M))
            //    playerObject.transform.position = transform.position; 


        }
    }
}
