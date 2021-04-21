using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputModeManager
{
    public static void SwitchInputModeMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameServices.moveController.DisableMovement();
        GameServices.cameraController.DisableControl();
    }

    public static void SwitchInputModeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameServices.moveController.EnableMovement();
        GameServices.cameraController.EnableControl();
    }
}
