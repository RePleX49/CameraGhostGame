using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    Slider sanitySlider;

    delegate float UpdateSliderPercentage();

    public Animator fadeOutAnim;
    public Image pillFillImage;

    public GameObject GameOverPanel;

    public AudioMixer audioMixer;
    public AudioMixerSnapshot unPausedSnapshot;
    public AudioMixerSnapshot muteSnapshot;

    // Start is called before the first frame update
    void Start()
    {
        GameServices.playerUI = this;
    }

    // Update is called once per frame
    void Update()
    {
        sanitySlider.value = GameServices.playerStats.GetSanityPercentage();
        pillFillImage.fillAmount = GameServices.playerStats.GetPillCountRatio();
    }

    // Trigger for fade to black effect
    public void PlayFadeOut()
    {
        fadeOutAnim.SetTrigger("FadeOut");
    }

    public IEnumerator ShowGameOver()
    {
        InputModeManager.SwitchInputModeMenu();
        PlayFadeOut();
        GameServices.gameCycleManager.gameOver = true;
        yield return new WaitForSeconds(2.0f);
        GameOverPanel.SetActive(true);
        yield return null;
    }
}
