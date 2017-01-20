using System;
using UnityEngine;

namespace Assets
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject Camera;

        float flySpeed = 10;
      
        bool shift;
        bool ctrl;
        float accelerationAmount = 30;
        float accelerationRatio = 3;
        float slowDownRatio = 0.2f;

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
            //use shift to speed up flight
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                shift = true;
                flySpeed *= accelerationRatio;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                shift = false;
                flySpeed /= accelerationRatio;
            }

            //use ctrl to slow up flight
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                ctrl = true;
                flySpeed *= slowDownRatio;
            }

            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            {
                ctrl = false;
                flySpeed /= slowDownRatio;
            }
            //
            if (Input.GetAxis("Vertical") != 0)
            {
                Camera.transform.Translate(Vector3.forward * flySpeed * Input.GetAxis("Vertical"));
            }


            if (Input.GetAxis("Horizontal") != 0)
            {
                Camera.transform.Translate(Vector3.right * flySpeed * Input.GetAxis("Horizontal"));
            }


            if (Input.GetKey(KeyCode.E))
            {
                Camera.transform.Translate(Vector3.up * flySpeed);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                Camera.transform.Translate(Vector3.down * flySpeed);
            }


            //if (Input.GetKeyDown(KeyCode.M))
            //    playerObject.transform.position = transform.position; 


        }
    }
}
