using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameServices
{
    public static CameraController cameraController;
    public static AudioController audioController;
    public static ResourceManager playerStats;
    public static GameCycleManager gameCycleManager;

    public static void Initialize()
    {
        playerStats = new ResourceManager();
        audioController = new AudioController();
    }
}
