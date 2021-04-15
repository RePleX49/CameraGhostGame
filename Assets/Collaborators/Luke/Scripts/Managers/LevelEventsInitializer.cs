using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventsInitializer : MonoBehaviour
{
    private void Start()
    {
        FindLevelEvent[] findEvents = FindObjectsOfType<FindLevelEvent>();
        foreach (FindLevelEvent findEvent in findEvents)
        {
            findEvent.SearchForNext();
        }
    }
}
