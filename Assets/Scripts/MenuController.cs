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
    public Animator cameraAC;
    public GameObject soundMenu;
    public Button continueButton;

    void Start()
    {
        mainMenu.SetActive(true);
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
        newGame = true;
        mainMenu.SetActive(false);
        cameraAC.SetBool("newGameAnim", true);
        startGame.SetActive(true);
    }

    //start from last save point
    public void Retry()
    {
        // Load section 1 or section 2 based on saved boolean
        PlayerSaveData saveData = new PlayerSaveData();
        saveData.SetFromString(ReadTextFile("","playerData.txt"));

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

    //exit to main menu
    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public string ReadTextFile(string filePath, string fileName)
    {
        var fileReader = new StreamReader(Application.dataPath + filePath + "/" + fileName);
        var toReturn = "";
        using (fileReader)
        {
            string line;
            do
            {
                line = fileReader.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    toReturn += line + '\n';
                }
            } while (line != null);

            fileReader.Close();
        }

        return toReturn;
    }
}
