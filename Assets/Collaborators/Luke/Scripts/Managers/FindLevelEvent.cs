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
        thisLevelEvent = GetComponent<LevelEvent>();
        thisLevelEvent.nextEvent = GameObject.Find(gameObjectName).GetComponent<LevelEvent>();
    }
}
