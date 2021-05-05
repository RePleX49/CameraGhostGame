using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private Boolean newGame;
    public GameObject mainMenu;
    public GameObject startGame;
    public GameObject confirmNewGame;
    public Animator cameraAC;
    public GameObject soundMenu;
    public Button continueButton;
    public SettingsMenuScript settingsMenu;
    public Text continueText;

    PlayerSaveData saveData;

    void Start()
    {
        saveData = new PlayerSaveData();

        if (File.Exists(Path.Combine(Application.dataPath, "playerData.txt")))
        {
            // set data of saveData from available file
            saveData.SetFromString(PlayerSaveData.ReadTextFile("", "playerData.txt"));
        }
        else
        {
            // make file with empty save
            PlayerSaveData.WriteString("playerData.txt", saveData.GetString());
        }

        if (mainMenu)
        {
            InputModeManager.SwitchInputModeMainMenu();
            mainMenu.SetActive(true);

            if(saveData.isCompletedSave)
            {
                continueButton.interactable = false;
                Color newColor = new Color(continueText.color.r, continueText.color.g, continueText.color.b, 0.3f);
                continueText.color = newColor;
            }          
        }
            
        newGame = false;

        if(startGame)
        {
            startGame.SetActive(false);
        }     
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (newGame)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
    
    //button methods
    
    //start a new game
    public void NewGame()
    {
        // Reset save data and set local data reference to new file
        PlayerSaveData.DeletePlayerSave();
        saveData.SetFromString(PlayerSaveData.ReadTextFile("", "playerData.txt"));
        saveData.isCompletedSave = false;
        PlayerSaveData.WriteString(PlayerSaveData.saveFileName, saveData.GetString());
        mainMenu.SetActive(false);
        CloseConfirmNewGame();
        cameraAC.SetBool("newGameAnim", true);
        Invoke("SetStartGameActive", 5.0f);
    }

    void SetStartGameActive()
    {
        startGame.SetActive(true);
        newGame = true;
    }

    //start from last save point
    public void Retry()
    {
        // Load section 1 or section 2 based on saved boolean
        if(saveData.clearedSection1 == true)
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        soundMenu.SetActive(true);
    }
    
    public void CloseOptions()
    {
        settingsMenu.SaveVolumePreference();
        mainMenu.SetActive(true);
        soundMenu.SetActive(false);
    }

    public void OpenConfirmNewGame()
    {
        saveData.SetFromString(PlayerSaveData.ReadTextFile("", "playerData.txt"));
        if (saveData.isCompletedSave)
        {
            NewGame();
        }
        else
        {
            confirmNewGame.SetActive(true);
        }     
    }

    public void CloseConfirmNewGame()
    {
        confirmNewGame.SetActive(false);
    }

    //exit to main menu
    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}