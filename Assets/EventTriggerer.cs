using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerer : MonoBehaviour
{
    public GameObject eventManager;
    public int eventNumber;

    // Start is called before the first frame update
    void Start()
    {
        eventNumber = int.Parse(this.gameObject.name.Substring(4, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            eventManager.SendMessage("Triggered", eventNumber);
        }
    }
}
