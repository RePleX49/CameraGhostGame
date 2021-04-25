using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    public Text mouseSenseText;
    public Text volumeText;

    public Slider mouseSenseSlider;
    public Slider masterVolumeSlider;

    public AudioMixer mainAudioMixer;

    private void Start()
    {
        // Load saved options settings
        float mixerVolume;
        PlayerSaveData saveData = PlayerSaveData.GetPlayerSave();
        mixerVolume = saveData.masterVolume;

        // reverse operations of mapping in AdjustMasterVolume function
        masterVolumeSlider.value = ((mixerVolume + 80.0f) / 80.0f) * 100.0f;
        AdjustMasterVolume(masterVolumeSlider.value);
    }

    public void ChangeMouseSense(float newSense)
    {
        GameServices.cameraController.mouseSensitivity = newSense;

        // also update mouse sense text in menu
        mouseSenseText.text = newSense.ToString("G2");
    }

    public void AdjustMasterVolume(float newVolume)
    {
        Debug.Log("Called adjust " + newVolume);
        // mapping 0->100 to (-80)->0 ("0->100" audio range in AudioMixer)
        newVolume /= 100.0f;
        float mixerVolume = (80.0f * newVolume) - 80.0f;

        mainAudioMixer.SetFloat("masterVolume", mixerVolume);

        int textVal = (int)masterVolumeSlider.value;
        volumeText.text = textVal.ToString("D");
    }
}
