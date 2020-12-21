using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] private AudioSource breathingSource;
    [SerializeField] private float maxVolume;
    [SerializeField] private float minVolume;

    [SerializeField] private float checkStep = .1f;

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if(GameManager.me.state == GameManager.me.game)
        {
            float mentalHealth = _gameManager.mentalHealth;

            if (mentalHealth > 0)
            {
                var percent = 1 - (mentalHealth / 100);
                var range = maxVolume - minVolume;

                breathingSource.volume = minVolume + (percent * range);
            }
        }
    }

    private IEnumerator InsanityCheck()
    {
        while (enabled)
        {
            Debug.Log("check");
            float mentalHealth = _gameManager.mentalHealth;
            
            if (mentalHealth > 0)
            {
                var percent = 1 - (mentalHealth / 100);
                var range = maxVolume - minVolume;

                breathingSource.volume = minVolume + (percent * range);
            
                yield return new WaitForSeconds(checkStep);
            }
        }
    }
}
