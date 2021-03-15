using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource ambientSource;

    // Start is called before the first frame update
    void Start()
    {
        GameServices.audioController = this;
        ambientSource = GetComponent<AudioSource>();
    }

    public void PlayAmbientNoise()
    {
        ambientSource.Play();
    }

    public void StopAmbientNoise()
    {
        ambientSource.Stop();
    }
}
