using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;

    //[SerializeField]
    public Camera currentCamera;

    [SerializeField]
    Camera altCamera;

    [SerializeField]
    GameObject dimensionCamRenderer;

    [SerializeField]
    GameObject cameraMesh;

    public float verticalEquipOffset;
    float initialEquipY;

    public float mouseSensitivity = 100.0f;
    public float maxLookUp = 90.0f;
    public float minLookDown = -90.0f;

    float lookUpRotation = 0.0f;

    public bool isDisabled = false;
    public bool isEquipped = false;
    public bool isFullyEquipped = false;

    public bool inRealLayer { get; private set; }

    const int alternateLayer = 12;

    // Start is called before the first frame update
    void Start()
    {
        GameServices.cameraController = this;
        Cursor.lockState = CursorLockMode.Locked;
        initialEquipY = cameraMesh.transform.localPosition.y + verticalEquipOffset;

        if(Player.gameObject.layer != alternateLayer)
        {
            inRealLayer = true;
        }
        else
        {
            inRealLayer = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        lookUpRotation -= mouseY;
        lookUpRotation = Mathf.Clamp(lookUpRotation, minLookDown, maxLookUp);

        currentCamera.transform.localRotation = Quaternion.Euler(lookUpRotation, 0.0f, 0.0f);
        altCamera.transform.localRotation = Quaternion.Euler(lookUpRotation, 0.0f, 0.0f);
        dimensionCamRenderer.transform.localRotation = Quaternion.Euler(lookUpRotation, 0.0f, 0.0f);
        Player.Rotate(Vector3.up * mouseX);

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

        if(Player.gameObject.layer == alternateLayer)
        {
            inRealLayer = false;
            GameServices.audioController.PlayAmbientNoise(); // Play Alternate Dimension Sounds
        }
        else
        {
            inRealLayer = true;
            GameServices.audioController.StopAmbientNoise(); // Play Normal Dimension Sounds
        }
    }

    IEnumerator SmoothEquip(Transform target, float initialOffset, float offsetScale)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < 1.0f)
        {
            float newY = initialOffset + EaseInOut(elapsedTime) * offsetScale;
            Vector3 newLocalPos = new Vector3(target.localPosition.x, newY, target.localPosition.z);

            target.localPosition = newLocalPos;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        isFullyEquipped = true;

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

    float EaseInOut(float time)
    {
        return -(Mathf.Cos(Mathf.PI * time) - 1) / 2;
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
