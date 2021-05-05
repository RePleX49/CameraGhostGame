using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillPickup : MonoBehaviour
{
    AudioSource audioSource;

    bool collected = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected)
            return;

        if(other.CompareTag("Player"))
        {
            collected = true;
            audioSource.Play();
            GameServices.playerStats.AddPills();
            Destroy(gameObject, 0.6f);
        }
    }
}
