using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    Slider sanitySlider;

    delegate float UpdateSliderPercentage();

    public Animator fadeOutAnim;
    public Text textPillCount;

    public GameObject GameOverPanel;

    // Start is called before the first frame update
    void Start()
    {
        GameServices.playerUI = this;
    }

    // Update is called once per frame
    void Update()
    {
        sanitySlider.value = GameServices.playerStats.GetSanityPercentage();
        textPillCount.text = GameServices.playerStats.GetPillCount().ToString();
    }

    // Trigger for fade to black effect
    public void PlayFadeOut()
    {
        fadeOutAnim.SetTrigger("FadeOut");
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0;
        InputModeManager.SwitchInputModeMenu();
        AudioListener.pause = true;
        GameOverPanel.SetActive(true);
    }
}
