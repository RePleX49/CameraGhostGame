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
            mainMenu.SetActive(true);

            if(saveData.isEmptySave)
            {
                continueButton.interactable = false;
            }          
        }
            
        newGame = false;
        startGame.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (newGame)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
    
    //button methods
    
    //start a new game
    public void NewGame()
    {
        PlayerSaveData.DeletePlayerSave();
        saveData.isEmptySave = false;
        PlayerSaveData.WriteString(PlayerSaveData.saveFileName, saveData.GetString());
        mainMenu.SetActive(false);
        CloseConfirmNewGame();
        cameraAC.SetBool("newGameAnim", true);
        Invoke("SetStartGameActive", 1.0f);
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
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        soundMenu.SetActive(true);
    }
    
    public void CloseOptions()
    {
        mainMenu.SetActive(true);
        soundMenu.SetActive(false);
    }

    public void OpenConfirmNewGame()
    {
        confirmNewGame.SetActive(true);
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