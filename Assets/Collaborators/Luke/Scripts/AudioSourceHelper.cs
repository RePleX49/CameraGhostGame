using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceHelper : MonoBehaviour
{
    public bool isNormalSource;

    // Start is called before the first frame update
    void Start()
    {
        if(isNormalSource)
        {
            GameServices.audioController.normalAudioSources.Add(GetComponent<AudioSource>());
        }
        else
        {
            GameServices.audioController.alternateAudioSources.Add(GetComponent<AudioSource>());
        }
    }
}
