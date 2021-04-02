using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelEventType
{
    DialogueOnly,
    Appear,
    Disappear,
    LevelTransition
}

[RequireComponent(typeof(SphereCollider))]
public class LevelEvent : MonoBehaviour
{
    public GameObject targetObject;
    public LevelEventType eventType;
    public LevelEvent nextEvent;

    public int transitionToSceneIndex;
    public float delayBeforeTransition;

    public TextBoxManager eventDialogue;

    private void Start()
    {
        targetObject = gameObject;
        switch (eventType)
        {
            case LevelEventType.Appear:
                targetObject.SetActive(false);
                break;
            case LevelEventType.Disappear:
                targetObject.SetActive(true);
                break;
        }
    }

    public void PlayLevelEvent()
    {
        switch(eventType)
        {
            case LevelEventType.Appear:
                targetObject.SetActive(true);
                break;
            case LevelEventType.Disappear:
                targetObject.SetActive(false);
                if(nextEvent)
                {
                    nextEvent.PlayLevelEvent();
                }
                
                break;
            case LevelEventType.LevelTransition:
                StartCoroutine(TransitionLevel());
                break;
            default:
                break;
        }

        if(eventDialogue)
        {
            eventDialogue.gameObject.SetActive(true);
            GameServices.cameraController.gameObject.GetComponent<PlayerMovement>().enabled = false;
            gameObject.SetActive(false);
        }
    }

    IEnumerator TransitionLevel()
    {
        GameServices.playerUI.PlayFadeOut();
        yield return new WaitForSeconds(delayBeforeTransition);
        SceneManager.LoadScene(transitionToSceneIndex);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayLevelEvent();
        }
    }
}
