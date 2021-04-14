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
    GameObject cameraMesh;

    [SerializeField]
    Animator cameraAnimator;

    public float verticalEquipOffset;
    float initialEquipY;

    public float mouseSensitivity = 100.0f;
    public float maxLookUp = 90.0f;
    public float minLookDown = -90.0f;

    float lookUpRotation = 0.0f;

    bool isDisabled = false;
    bool isEquipped = false;
    bool isTransitioning = false;

    public bool inRealLayer { get; private set; }

    const int alternateLayer = 12;

    public bool hasCamera = false;

    private void Awake()
    {
        GameServices.cameraController = this;
    }

    // Start is called before the first frame update
    void Start()
    {      
        Cursor.lockState = CursorLockMode.Locked;
        initialEquipY = cameraMesh.transform.localPosition.y + verticalEquipOffset;

        if(Player.gameObject.layer != alternateLayer)
        {
            inRealLayer = true;
            GameServices.audioController.SwitchToNormalAudio();
        }
        else
        {
            inRealLayer = false;
            GameServices.audioController.SwitchToAlternateAudio();
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
        Player.Rotate(Vector3.up * mouseX);

        if (isDisabled)
            return;

        /*
        if(Input.GetKeyDown(KeyCode.E))
        {
            SwapCameras();
        }
        */

        if(Input.GetKeyDown(KeyCode.Q) && hasCamera)
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

    // Same as "switching dimensions"
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

        // Swap audio based on current layer
        if(Player.gameObject.layer == alternateLayer)
        {
            inRealLayer = false;
            //GameServices.audioController.PlayAmbientNoise(); // Play Alternate Dimension Sounds
            GameServices.audioController.SwitchToAlternateAudio();
        }
        else
        {
            inRealLayer = true;
            //GameServices.audioController.StopAmbientNoise(); // Play Normal Dimension Sounds
            GameServices.audioController.SwitchToNormalAudio();
        }
    }

    IEnumerator SmoothEquip(Transform target, float initialOffset, float offsetScale)
    {
        float elapsedTime = 0.0f;
        isTransitioning = true;

        while (elapsedTime < 1.0f)
        {
            float newY = initialOffset + EaseInOut(elapsedTime) * offsetScale;
            Vector3 newLocalPos = new Vector3(target.localPosition.x, newY, target.localPosition.z);

            target.localPosition = newLocalPos;
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        isTransitioning = false;

        yield return null;
    }

    public void EquipCamera()
    {
        if(cameraAnimator.IsInTransition(0))
        {
            return;
        }

        // StartCoroutine(SmoothEquip(cameraMesh.transform, initialEquipY - verticalEquipOffset, verticalEquipOffset));
        cameraAnimator.SetTrigger("Equip");
        
        isEquipped = true;
    }

    public void UnequipCamera()
    {
        if (cameraAnimator.IsInTransition(0))
        {
            return;
        }

        //StartCoroutine(SmoothEquip(cameraMesh.transform, initialEquipY, -verticalEquipOffset));
        cameraAnimator.SetTrigger("Unequip");
        isEquipped = false;
    }

    public bool IsCameraReady()
    {
        return isEquipped && !isTransitioning;
    }

    public void DisableCamera()
    {
        isDisabled = true;
    }

    public void EnableCamera()
    {
        isDisabled = false;
    }

    // Easing functions
    // ****************
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

    public GameObject GetCameraMesh()
    {
        return cameraMesh;
    }
}
