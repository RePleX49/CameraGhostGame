using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceManager
{
    float currentSanity = 50.0f;
    float maxSanity = 50.0f;

    //float sanityDrainRateNormal = 0.2f;
    //float sanityDrainRateGhost = 0.4f;

    int pillsCount = 0;
    public bool clearedSection1 = false;

    private static PlayerSaveData playerData;

    public ResourceManager()
    {
        // load save data here
        Debug.Log("Would load save data in resource manager here");
    }

    public void Initialize()
    {
        playerData = new PlayerSaveData();

        if(!File.Exists(Path.Combine(Application.dataPath, "playerData.txt")))
        {
            pillsCount = 0;
            Debug.LogWarning("No player save file was found");
        }

        playerData.SetFromString(PlayerSaveData.ReadTextFile("", "playerData.txt"));
        Debug.Log("Pill Count: " + playerData.pillsCollected);
        pillsCount = playerData.pillsCollected;
        clearedSection1 = playerData.clearedSection1;
    }

    

    public void Update(float rechargeRate, float ghostRate)
    {
        if (currentSanity <= 0.0f)
        {
            // add fade out
            InputModeManager.SwitchInputModeMenu();
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
            return;
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

    public void ConsumePill()
    {
        if (pillsCount < 1)
            return;

        currentSanity = Mathf.Min(maxSanity, currentSanity + (maxSanity * 0.3f));
    }

    public void AddPills()
    {
        pillsCount++;
    }

    public void SubtractPills()
    {
        pillsCount--;
    }

    public int GetPillCount()
    {
        return pillsCount;
    }

    public void SavePlayerData()
    {
        playerData.pillsCollected = pillsCount;
        playerData.clearedSection1 = clearedSection1;
        PlayerSaveData.WriteString("playerData.txt", playerData.GetString());
    }
}
