using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public bool paused;
    public GameObject pauseScreen;
    private bool pauseScreenActive;
    public GameObject optionScreen;
    public GameObject controlScreen;
    public GameObject soundScreen;
    
    void Update()
    {
        Pause();
    }

    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0 && pauseScreen.activeSelf == false)
            {
                pauseScreen.SetActive(true);
                paused = true;
                pauseScreenActive = true;

                Time.timeScale = 0;
                InputModeManager.SwitchInputModeMenu();
            }
            else
            {
                UnPause();              
            }
        }
    }

    void UnPause()
    {
        if (pauseScreen.activeSelf == true)
        {
            pauseScreen.SetActive(false);
            paused = false;
            pauseScreenActive = false;
            optionScreen.SetActive(false);
            controlScreen.SetActive(false);

            Time.timeScale = 1;
            InputModeManager.SwitchInputModeGame();
        }
    }
    
    //button methods
    public void Resume()
    {
        UnPause();
    }

    public void OpenOptions()
    {
        pauseScreen.SetActive(false);
        optionScreen.SetActive(true);
    }
    
    public void CloseOptions()
    {
        pauseScreen.SetActive(true);
        optionScreen.SetActive(false);
    }
    
    public void MainMenu()
    {
        // Reset time scale so there are no issues when loading scene from main menu
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenControls()
    {
        optionScreen.SetActive(false);
        controlScreen.SetActive(true);
    }
    
    public void CloseControls()
    {
        optionScreen.SetActive(true);
        controlScreen.SetActive(false);
    }
    
    public void OpenSound()
    {
        optionScreen.SetActive(false);
        soundScreen.SetActive(true);
    }
    
    public void CloseSound()
    {
        optionScreen.SetActive(true);
        soundScreen.SetActive(false);
    }
}

