using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCycleManager : MonoBehaviour
{
    public float sanityDrainRateNormal = 0.2f;
    public float sanityDrainRateGhost = 0.4f;

    public bool inDialogue;

    // Start is called before the first frame update
    void Start()
    {
        inDialogue = false;
        GameServices.gameCycleManager = this;
        GameServices.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if(inDialogue)
        {
            return;
        }

        GameServices.playerStats.Update(sanityDrainRateNormal, sanityDrainRateGhost);
    }

    public void PauseUpdate()
    {
        inDialogue = true;
    }

    public void ResumeUpdate()
    {
        inDialogue = false;
    }
}
