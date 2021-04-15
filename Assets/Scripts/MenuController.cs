using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private Boolean newGame;
    public GameObject mainMenu;
    public GameObject startGame;
    public Animator cameraAC;
    public GameObject soundMenu;
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
                SceneManager.LoadScene("NewBlockoutREVISE");
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
}
