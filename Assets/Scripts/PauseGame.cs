using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseGame : MonoBehaviour
{
    public bool paused;
    public GameObject pauseScreen;
    private bool pauseScreenActive;
    public GameObject optionScreen;
    public GameObject controlScreen;
    public GameObject soundScreen;
    public AudioMixerSnapshot pausedAudioSnapshot;
    public AudioMixerSnapshot unpausedAudioSnapshot;
    public AudioMixer audioMixer;
    public SettingsMenuScript settingsMenu;
    
    void Update()
    {
        Pause();
    }

    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameServices.gameCycleManager.gameOver)
                return;

            if (Time.timeScale != 0 && pauseScreen.activeSelf == false)
            {
                // AudioListener.pause = true;
                TransitionSnapshots(unpausedAudioSnapshot, pausedAudioSnapshot, 0.1f);
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
            // AudioListener.pause = false;
            TransitionSnapshots(pausedAudioSnapshot, unpausedAudioSnapshot, 0.1f);
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
        unpausedAudioSnapshot.TransitionTo(0.0f);
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
        settingsMenu.SaveVolumePreference();
        optionScreen.SetActive(true);
        soundScreen.SetActive(false);
    }

    // Transition Snapshot code to circumvent Timescale = 0
    private Coroutine transitionCoroutine;
    private AudioMixerSnapshot endSnapshot;

    public void TransitionSnapshots(AudioMixerSnapshot fromSnapshot, AudioMixerSnapshot toSnapshot, float transitionTime)
    {
        EndTransition();
        transitionCoroutine = StartCoroutine(TransitionSnapshotsCoroutine(fromSnapshot, toSnapshot, transitionTime));
    }

    IEnumerator TransitionSnapshotsCoroutine(AudioMixerSnapshot fromSnapshot, AudioMixerSnapshot toSnapshot, float transitionTime)
    {
        // transition values
        int steps = 20;
        float timeStep = (transitionTime / (float)steps);
        float transitionPercentage = 0.0f;
        float startTime = 0f;

        // set up snapshots
        endSnapshot = toSnapshot;
        AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[] { fromSnapshot, toSnapshot };
        float[] weights = new float[2];

        // stepped-transition
        for (int i = 0; i < steps; i++)
        {
            transitionPercentage = ((float)i) / steps;
            weights[0] = 1.0f - transitionPercentage;
            weights[1] = transitionPercentage;
            audioMixer.TransitionToSnapshots(snapshots, weights, 0f);

            // this is required because WaitForSeconds doesn't work when Time.timescale == 0
            startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup < (startTime + timeStep))
            {
                yield return null;
            }
        }

        // finalize
        EndTransition();
    }

    void EndTransition()
    {
        if ((transitionCoroutine == null) || (endSnapshot == null))
        {
            return;
        }

        StopCoroutine(transitionCoroutine);
        endSnapshot.TransitionTo(0f);
    }
}

