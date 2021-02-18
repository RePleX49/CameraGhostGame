using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    Camera currentCamera;
    public Camera altCamera { get; private set; }
    public Camera defaultCamera;
    public Camera ghostCamera;
    public float mouseSensitivity = 100.0f;
    public float maxLookUp = 90.0f;
    public float minLookDown = -90.0f;

    float lookUpRotation = 0.0f;

    void Awake()
    {
        currentCamera = defaultCamera;
        altCamera = ghostCamera;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameServices.cameraController = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        lookUpRotation -= mouseY;
        lookUpRotation = Mathf.Clamp(lookUpRotation, minLookDown, maxLookUp);

        defaultCamera.transform.localRotation = Quaternion.Euler(lookUpRotation, 0.0f, 0.0f);
        ghostCamera.transform.localRotation = Quaternion.Euler(lookUpRotation, 0.0f, 0.0f);
        Player.Rotate(Vector3.up * mouseX);
    }
}
