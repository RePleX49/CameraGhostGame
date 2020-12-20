using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// called by ObjectScript
// make sure play on awake is set to false in editor!
[RequireComponent(typeof(AudioSource))]
public class ObjectAudio : MonoBehaviour
{
    [SerializeField] private bool testMode = false; // plays on game start if enabled
    
    [SerializeField] private AudioClip startClip = null;
    [SerializeField] private float startVolume = 1f;
    
    [SerializeField] private AudioClip constantClip = null;
    [SerializeField] private float constantVolume = 1;
    
    [SerializeField] private AudioClip endClip = null;
    [SerializeField] private float endVolume = 1f;
    
    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.spatialBlend = 1;
        audioSource.volume = constantVolume;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.minDistance = 2;
        audioSource.maxDistance = 700;
        
        audioSource.clip = constantClip;

        if (testMode)
        {
            StartCoroutine(Test(3.5f));
        }
    }

    private IEnumerator Test(float duration)
    {
        PlayStart();
        PlayConstant();
        yield return new WaitForSeconds(duration);
        StopConstant();
        PlayEnd();
    }
    
    public void PlayStart()
    {
        if (startClip)
        {
            audioSource.PlayOneShot(startClip, startVolume);
        }
    }

    public void PlayConstant()
    {
        if (audioSource.clip)
        {
            audioSource.Play();
        }
        else
        {
            Debug.Log("No constant audio clip found on " + gameObject.name);
        }
    }
    
    public void StopConstant()
    {
        audioSource.Stop();
    }

    public void PlayEnd()
    {
        if (endClip)
        {
            audioSource.PlayOneShot(endClip, endVolume);
        }
    }
}
