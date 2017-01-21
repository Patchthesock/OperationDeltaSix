using System;
using UnityEngine;

namespace Assets
{
    public class CameraManager : MonoBehaviour
    {
        public GameObject Camera;

        private bool _shift;
        private bool _ctrl;
        private float _flySpeed = 0.5f;
        private float _accelerationAmount = 30;
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
                _flySpeed *= _accelerationRatio;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                _shift = false;
                _flySpeed /= _accelerationRatio;
            }

            //use ctrl to slow up flight
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                _ctrl = true;
                _flySpeed *= _slowDownRatio;
            }

            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            {
                _ctrl = false;
                _flySpeed /= _slowDownRatio;
            }
            
            if (Math.Abs(Input.GetAxis("Vertical")) > 0.01)
            {
                //var newForward = new Vector3(Vector3.forward.x,Vector3.forward.y, 0);
                //var pos = Vector3.forward * _flySpeed * Input.GetAxis("Vertical");
                Camera.transform.position = Camera.transform.position + (new Vector3(1, 0, 1) * Input.GetAxis("Vertical"));
                //Camera.transform.Translate(new Vector3(pos.x, 0, pos.z));
                //Camera.transform.position = Camera.transform.position + new Vector3(1, 0, 0) * Input.GetAxis("Vertical");
            }

            //if (Math.Abs(Input.GetAxis("Horizontal")) > 0.01)
            //{
            //    //Camera.transform.Translate(Vector3.right * _flySpeed * Input.GetAxis("Horizontal"));
            //    Camera.transform.position = Camera.transform.position + new Vector3(0, 0, 1) * Input.GetAxis("Horizontal");
            //}

            if (Input.GetKey(KeyCode.E))
            {
                Camera.transform.Translate(Vector3.up * _flySpeed);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                Camera.transform.Translate(Vector3.down * _flySpeed);
            }


            //if (Input.GetKeyDown(KeyCode.M))
            //    playerObject.transform.position = transform.position; 


        }
    }
}
