using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public GameObject eventManager;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameServices.cameraController.SwapCameras();
            eventManager.SendMessage("EnteredPortal");
        }
    }
}
