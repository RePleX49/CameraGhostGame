﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCycleManager : MonoBehaviour
{
    public float sanityRechargeRate = 0.2f;
    public float sanityDrainRateGhost = 0.4f;

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
        sanityDrainRateGhost = 2.4f;
    }

    public void BeingHunted2()
    {
        sanityDrainRateGhost = 6.0f;
    }

    public void ResetDrainRate()
    {
        sanityDrainRateGhost = 0.4f;
    }
}