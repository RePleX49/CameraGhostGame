using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLevelEvent : MonoBehaviour
{
    public string gameObjectName;

    // the level event on the gameobject this script is attached to
    LevelEvent thisLevelEvent;

    // Start is called before the first frame update
    void Start()
    {
             
    }

    public void SearchForNext()
    {
        thisLevelEvent = GetComponent<LevelEvent>();
        GameObject nextEventObject = GameObject.Find(gameObjectName);
        Debug.Log(nextEventObject);
        LevelEvent nextLevelEvent = nextEventObject.GetComponent<LevelEvent>();
        thisLevelEvent.nextEvent = nextLevelEvent;
    }
}
