using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    Slider sanitySlider;

    public Animator fadeOutAnim;

    // Start is called before the first frame update
    void Start()
    {
        GameServices.playerUI = this;
    }

    // Update is called once per frame
    void Update()
    {
        sanitySlider.value = GameServices.playerStats.GetSanityPercentage();
    }

    public void PlayFadeOut()
    {
        fadeOutAnim.SetTrigger("FadeOut");
    }
}
