//using UnityEngine;

//public class MouseLook : MonoBehaviour
//{
//    [HideInInspector]
//    public static MouseLook instance = null;

//    public void SetActive(bool state)
//    {
//        if (state)
//        {
//            originalRotation = transform.localRotation;
//            _isActive = true;
//        }
//        else
//        {
//            _isActive = false;
//        }
//    }

//    private void Awake()
//    {
//        if (instance == null)
//            instance = this;
//        else if (instance != this)
//            Destroy(gameObject);

//        DontDestroyOnLoad(gameObject);
//        _isActive = false;
//    }

//    public enum RotationAxes
//    {
//        MouseXAndY = 0,
//        MouseX = 1,
//        MouseY = 2
//    }

//    public RotationAxes axes = RotationAxes.MouseXAndY;
//    public float sensitivityX = 15F;
//    public float sensitivityY = 15F;
//    public float minimumX = -360F;
//    public float maximumX = 360F;
//    public float minimumY = -60F;
//    public float maximumY = 60F;
//    public GameObject Camera;
//    float rotationX = 0F;
//    float rotationY = 0F;
//    Quaternion originalRotation;
//    private bool _isActive;

//    private void Update()
//    {
//        if (!_isActive) return;
//        switch (axes)
//        {
//                case RotationAxes.MouseXAndY:
//                {
//                    // Read the mouse input axis
//                    rotationX += Input.GetAxis("Mouse X") * sensitivityX;
//                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
//                rotationX = ClampAngle(rotationX, minimumX, maximumX);
//                rotationY = ClampAngle(rotationY, minimumY, maximumY);
//                var xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
//                var yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
//                Camera.transform.rotation = originalRotation * xQuaternion * yQuaternion;
//            }
//                break;
//            case RotationAxes.MouseX:
//            {
//                rotationX += Input.GetAxis("Mouse X") * sensitivityX;
//                rotationX = ClampAngle(rotationX, minimumX, maximumX);
//                Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
//                Camera.transform.rotation = originalRotation * xQuaternion;
//            }
//                break;
//            case RotationAxes.MouseY:
//                break;
//            default:
//            {
//                rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
//                rotationY = ClampAngle(rotationY, minimumY, maximumY);
//                var yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
//                Camera.transform.rotation = originalRotation * yQuaternion;
//            }
//                break;
//        }
//    }

//    private void Start()
//    {
//        originalRotation = transform.localRotation;
//    }

//    private static float ClampAngle(float angle, float min, float max)
//    {
//        if (angle < -360F)
//        angle += 360F;
//        if (angle > 360F)
//        angle -= 360F;
//        return Mathf.Clamp(angle, min, max);
//    }
//}