using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameCycleManager : MonoBehaviour
{
    public float sanityRechargeRate = 0.2f;
    public float sanityDrainRateGhost = 0.4f;

    public AudioMixer mainAudioMixer;

    bool inDialogue;

    // Start is called before the first frame update
    void Awake()
    {
        inDialogue = false;
        GameServices.gameCycleManager = this;
        GameServices.Initialize();
        GameServices.playerStats.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        float volumePercentage = GameServices.playerStats.GetSanityPercentage();
        volumePercentage = 1 - volumePercentage;
        float mixerVolume = (80.0f * volumePercentage) - 80.0f;
        mainAudioMixer.SetFloat("sanityBGMVolume", mixerVolume);

        if(inDialogue)
        {
            return;
        }

        GameServices.playerStats.Update(sanityRechargeRate, sanityDrainRateGhost);
    }

    public void PauseUpdate()
    {
        inDialogue = true;
    }

    public void ResumeUpdate()
    {
        inDialogue = false;
    }

    public void BeingHunted1()
    {
        sanityDrainRateGhost = 1.2f;
    }

    public void BeingHunted2()
    {
        sanityDrainRateGhost = 4.5f;
    }

    public void ResetDrainRate()
    {
        sanityDrainRateGhost = 0.4f;
    }
}
