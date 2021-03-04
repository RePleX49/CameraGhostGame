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
    public GameObject cameraObject;
    public GameObject cameraMesh;
    public float verticalEquipOffset;
    float initialEquipY;
    public float mouseSensitivity = 100.0f;
    public float maxLookUp = 90.0f;
    public float minLookDown = -90.0f;

    float lookUpRotation = 0.0f;

    bool isDisabled = false;
    bool isEquipped = true;

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
        initialEquipY = cameraMesh.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDisabled)
            return;

        //if(Input.GetKeyDown(KeyCode.E))
        //{
        //    SwapCameras();
        //}

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(isEquipped)
            {
                UnequipCamera();
            }
            else
            {
                EquipCamera();
            }
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        lookUpRotation -= mouseY;
        lookUpRotation = Mathf.Clamp(lookUpRotation, minLookDown, maxLookUp);

        defaultCamera.transform.localRotation = Quaternion.Euler(lookUpRotation, 0.0f, 0.0f);
        ghostCamera.transform.localRotation = Quaternion.Euler(lookUpRotation, 0.0f, 0.0f);
        cameraObject.transform.localRotation = Quaternion.Euler(lookUpRotation, 0.0f, 0.0f);
        Player.Rotate(Vector3.up * mouseX);
    }

    public void SwapCameras()
    {
        // Swap references for current and altCamera
        Camera temp = currentCamera;
        currentCamera = altCamera;
        altCamera = temp;

        // Swap render texture
        altCamera.tag = "Untagged";
        altCamera.targetTexture = currentCamera.targetTexture;
        currentCamera.targetTexture = null;
        currentCamera.tag = "MainCamera";

        // Match collision layer to current camera layer
        Player.gameObject.layer = currentCamera.gameObject.layer;
    }

    IEnumerator SmoothEquip(Transform target, float initialOffset, float offsetScale)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < 1.0f)
        {
            float newY = initialOffset + EaseIn(elapsedTime) * offsetScale;
            Vector3 newLocalPos = new Vector3(target.localPosition.x, newY, target.localPosition.z);

            target.localPosition = newLocalPos;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        yield return null;
    }

    public void EquipCamera()
    {
        StartCoroutine(SmoothEquip(cameraMesh.transform, initialEquipY - verticalEquipOffset, verticalEquipOffset));
        isEquipped = true;
    }

    public void UnequipCamera()
    {
        StartCoroutine(SmoothEquip(cameraMesh.transform, initialEquipY, -verticalEquipOffset));
        isEquipped = false;
    }

    float EaseIn(float time)
    {
        return 1 - Mathf.Cos((time * Mathf.PI) / 2);
    }

    float EaseOut(float time)
    {
        return Mathf.Sin((time * Mathf.PI) / 2);
    }

    public void DisableCamera()
    {
        isDisabled = true;
    }

    public void EnableCamera()
    {
        isDisabled = false;
    }
}
