using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCycleManager : MonoBehaviour
{
    public float sanityDrainRateNormal = 0.2f;
    public float sanityDrainRateGhost = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        GameServices.gameCycleManager = this;
        GameServices.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        GameServices.playerStats.Update(sanityDrainRateNormal, sanityDrainRateGhost);
    }
}
