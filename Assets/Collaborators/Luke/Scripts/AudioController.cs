using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController
{
    public List<AudioSource> normalAudioSources = new List<AudioSource>();
    public List<AudioSource> alternateAudioSources = new List<AudioSource>();

    public void SwitchToNormalAudio()
    {
        foreach(AudioSource source in alternateAudioSources)
        {
            source.Stop();
        }

        foreach (AudioSource source in normalAudioSources)
        {
            source.Play();
        }
    }

    public void SwitchToAlternateAudio()
    {
        foreach (AudioSource source in alternateAudioSources)
        {
            source.Play();
        }

        foreach (AudioSource source in normalAudioSources)
        {
            source.Stop();
        }
    }
}
