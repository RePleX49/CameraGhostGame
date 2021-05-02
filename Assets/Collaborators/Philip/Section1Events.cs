using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section1Events : MonoBehaviour
{
    public GameObject text1, text2, text3, text4, text5, text6, text7;

    public GameObject cameraObject;
    public GameObject portal;

    public PlayerMovement playerMovement;
    public CameraController cameraController;
    public GameObject ghostCameraObject;

    public Camera portalCamera;

    public bool talking;
    public bool[] eventTriggered = new bool[7];

    // Start is called before the first frame update
    void Start()
    {
        GameServices.gameCycleManager.PauseUpdate();
        StartCoroutine(LateStart()); //TODO refactor not actually a LateStart
    }

    // Update is called once per frame
    void Update()
    {
        if (talking)
        {
            
        }
        else
        {
            
        }

        if (GameServices.cameraController.IsCameraReady() && eventTriggered[1] && !eventTriggered[2])
        {
            GameServices.cameraController.DisableCamera();
            playerMovement.DisableMovement();
            text3.SetActive(true);
        }
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.5f);
        playerMovement = GameObject.Find("Player(Clone)").GetComponent<PlayerMovement>();
        cameraController = GameServices.cameraController;
        ghostCameraObject = GameServices.cameraController.GetCameraMesh();
        cameraController.DisableCamera();
        playerMovement.DisableMovement();
        text1.SetActive(true);
        talking = true;
    }

    public void FinishDialogue(int textNumber)
    {
        if(textNumber == 2)
        {
            cameraObject.gameObject.SetActive(false);
            cameraController.EnableCamera();
        }
        else if(textNumber == 4)
        {
            StartCoroutine(ShowPortalAppearance());
            eventTriggered[3] = true;
            return;
        }
        else if(textNumber >= 3)
        {
            cameraController.EnableCamera();
            if(textNumber == 6)
            {
                GameServices.gameCycleManager.ResumeUpdate();
            }
        }
        eventTriggered[textNumber - 1] = true;
        talking = false;
        GameServices.gameCycleManager.ResumeUpdate();
        playerMovement.EnableMovement();
    }

    public void Triggered(int eventNumber)
    {
        if (!eventTriggered[eventNumber - 1])
        {
            if (eventNumber == 2)
            {
                cameraController.DisableCamera();
                playerMovement.DisableMovement();
                talking = true;
                text2.SetActive(true);
            }
            if(eventNumber == 4 && eventTriggered[2]) //&& cameraController.IsCameraReady())
            {
                playerMovement.DisableMovement();
                talking = true;
                text4.SetActive(true);
            }
            if (eventNumber == 7)
            {
                playerMovement.DisableMovement();
                talking = true;
                text7.SetActive(true);
            }
        }
    }

    IEnumerator ShowPortalAppearance()
    {
        cameraController.currentCamera.enabled = false;
        ghostCameraObject.SetActive(false);
        portalCamera.enabled = true;
        yield return new WaitForSeconds(2f);
        portal.SetActive(true);
        yield return new WaitForSeconds(2f);
        cameraController.currentCamera.enabled = true;
        ghostCameraObject.SetActive(true);
        portalCamera.enabled = false;
        text5.SetActive(true);
        yield return null;
    }

    public void EnteredPortal()
    {
        if (!eventTriggered[5])
        {
            playerMovement.DisableMovement();
            talking = true;
            text6.SetActive(true);
        }
    }
}
