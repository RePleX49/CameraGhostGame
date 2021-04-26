using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostWalkOff : MonoBehaviour
{
    public Vector3 destination;
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WalkOff()
    {
        agent.SetDestination(destination);
    }
}
