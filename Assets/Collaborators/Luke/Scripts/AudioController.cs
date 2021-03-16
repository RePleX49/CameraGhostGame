using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //AudioSource ambientSource;
    public AudioClip[] normal_dimension;
    public AudioClip[] alternate_dimension;
    public AudioSource normal_source;
    public AudioSource alternate_source;


    // Start is called before the first frame update
    void Start()
    {
        GameServices.audioController = this;
        AudioSource[] allAudioSources = GetComponents<AudioSource>();
        normal_source = allAudioSources[0];
        alternate_source = allAudioSources[1];
        //ambientSource = GetComponent<AudioSource>();
    }

    public void play_normal_Audio(int clipNumber)
    {
        normal_source.clip = normal_dimension[clipNumber];
        alternate_source.Stop();
        normal_source.Play();
    }

    public void play_alternate_Audio(int clipNumber)
    {
        alternate_source.clip = alternate_dimension[clipNumber];
        normal_source.Stop();
        alternate_source.Play();
    }

    public void PlayAmbientNoise()
    {
        //ambientSource.Play();
    }

    public void StopAmbientNoise()
    {
        //ambientSource.Stop();
    }
}
