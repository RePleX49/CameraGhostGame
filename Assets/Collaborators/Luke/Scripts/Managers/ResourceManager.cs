using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    float currentSanity = 50.0f;
    float maxSanity = 50.0f;

    //float sanityDrainRateNormal = 0.2f;
    //float sanityDrainRateGhost = 0.4f;

    int pillsCount = 0;

    public ResourceManager()
    {
        // load save data here
        Debug.Log("Would load save data in resource manager here");
    }

    public void Update(float rechargeRate, float ghostRate)
    {
        if (currentSanity <= 0.0f)
        {
            return;
            // call day end with penalty
        }

        float drainRate;

        if(GameServices.cameraController.inRealLayer)
        {
            drainRate = -rechargeRate;
        }
        else
        {
            drainRate = ghostRate;
        }

        currentSanity = Mathf.Clamp(currentSanity - drainRate * Time.deltaTime, 0.0f, maxSanity);  
    }

    public void ChangeSanity(float changeValue)
    {
        currentSanity = Mathf.Clamp(currentSanity + changeValue, 0.0f, maxSanity);
    }

    public float GetSanity()
    {
        return currentSanity;
    }

    public float GetSanityPercentage()
    {
        return currentSanity / maxSanity;
    }
}
