using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostVision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("oh");
            SendMessageUpwards("DetectPlayer");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            SendMessageUpwards("UndetectPlayer");
        }
    }
}
