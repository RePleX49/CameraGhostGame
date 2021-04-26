using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameServices.playerStats.AddPills();
            Destroy(gameObject);
        }
    }
}
