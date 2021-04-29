using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillPickup : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            audioSource.Play();
            GameServices.playerStats.AddPills();
            Destroy(gameObject, 0.65f);
        }
    }
}
