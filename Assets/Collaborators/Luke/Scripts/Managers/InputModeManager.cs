using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SwitchInputModeMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        GameServices.moveController.DisableMovement();
    }

    public void SwitchInputModeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameServices.moveController.EnableMovement();
    }
}
