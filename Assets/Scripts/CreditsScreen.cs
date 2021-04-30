using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScreen : MonoBehaviour
{
    public Animator fadeAC;
    public GameObject credits;
    public GameObject toBeContinued;
    
    public bool onCreditsScreen;
    public bool fadeStart;
    public bool onEndScreen;
    public bool canEnd;

    public float timer;
    
    void Start()
    {
        onCreditsScreen = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            //fade to black from credits screen
            if (onCreditsScreen) 
            { 
                fadeAC.SetBool("CloseCredits", true); 
                onCreditsScreen = false;
            }
            //fade to black from end screen
            if (canEnd)
            {
                fadeAC.SetBool("CloseCredits", true);
            }
        }
        
        //check if the screen has faded to black
        if (fadeAC.GetCurrentAnimatorStateInfo(0).IsName("Fadein"))
        {
            fadeStart = true;
        }
        
        //turn off the credits screen, fade in to end screen
        if (credits.active && onCreditsScreen==false && fadeStart && (!canEnd)) 
        {
            if (!(fadeAC.GetCurrentAnimatorStateInfo(0).IsName("Fadein")))
            {
                credits.SetActive(false);
                fadeAC.SetBool("EndSceneReady", true);
                toBeContinued.SetActive(true);
                fadeAC.SetBool("CloseCredits", false);
                fadeStart = false;
                onEndScreen = true;
            }
        }
        //once faded to black, exit to main menu
        else if (onCreditsScreen==false && fadeStart) 
        {
            if (!(fadeAC.GetCurrentAnimatorStateInfo(0).IsName("Fadein")))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        
        //start a timer once on end screen, once it hits 0 let the player exit to the main screen
        if (onEndScreen)
        {
            timer = timer - 1 * Time.deltaTime;
            if (timer<=0)
            {
                canEnd = true;
            }
        }
    }
}
