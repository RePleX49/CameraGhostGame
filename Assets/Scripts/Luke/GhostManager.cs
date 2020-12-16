using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public List<ObjectScript> hauntObjects;
    public GameObject ghostPrefab;

    public float ghostInterval = 10.0f;

    float timer;
    bool bGhostSpawned = false;

    List<GameObject> activeGhosts;

    // Start is called before the first frame update
    void Start()
    {
        timer = ghostInterval;
        activeGhosts = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.me.state == GameManager.me.game)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                if(!bGhostSpawned)
                {
                    // need to clamp ghost spawn count to hauntObject count
                    int ghostsToSpawn = (100 / GameManager.me.mentalHealth);

                    //TODO use a for loop to spawn ghosts
                    for(int i = 0; i < 3; i++)
                    {
                        GameObject ghost = Instantiate(ghostPrefab);
                        ghost.GetComponent<GhostScript>().Initialize(this, GetHaunt());
                        activeGhosts.Add(ghost);
                    }                 

                    bGhostSpawned = true;
                    timer = ghostInterval;
                }
                else
                {
                    ClearAllGhosts();
                    bGhostSpawned = false;
                    timer = ghostInterval;
                }
            }        
        }

        Debug.Log("Current time " + timer);
    }

    void ClearAllGhosts()
    {
        foreach(GameObject ghost in activeGhosts)
        {
            ghost.GetComponent<GhostScript>().ResetHaunt();
            hauntObjects.Add(ghost.GetComponent<GhostScript>().GetHaunt());
            activeGhosts.Remove(ghost);
            Destroy(ghost);
        }
    }

    public void ClearGhost(GameObject target, ObjectScript targetHaunt)
    {
        hauntObjects.Add(targetHaunt);
        activeGhosts.Remove(target);
    }

    ObjectScript GetHaunt()
    {
        int index = Random.Range(0, hauntObjects.Count);

        ObjectScript hauntObject = hauntObjects[index];
        hauntObjects.RemoveAt(index);

        return hauntObject;
    }
}
