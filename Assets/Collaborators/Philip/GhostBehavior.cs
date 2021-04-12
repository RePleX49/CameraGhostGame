using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostBehavior : MonoBehaviour
{
    // NECESSARY COMPONENT FOR PATH FINDING
    public NavMeshAgent agent;

    // WE USE A VECTOR3 ARRAY TO STORE AND CYCLE THROUGH DIFFERENT LOCATIONS ON ITS PATROLLING ROUTE
    public Vector3[] spots;
    public int currentSpotNumber;       // mostly used for index of spots array

    // THIS BOOL IS USED TO CHECK WHEN A GHOST HAS RETURNED FROM A "HUNTING DETOUR" AND CAN CONTINUE PATROLLING
    public bool backtracked = true;

    public GameObject player;

    // DETECTION IS A STATE WHEN THE GHOST VISIBLY SEES THE PLAYER
    public bool detected;               // does the ghost see the player?
    public float detectionTime;         // the maximum amount of time the ghost has until forgetting about the player
    public float currentDetectionTime;  // the amount of time the ghost currently has until forgetting about the player... think of it as like a memory stat for the ghost

    // ALERTNESS IS A STATE WHEN THE GHOST IS STILL THINKING ABOUT THE PLAYER, EVEN IF IT CANNOT SEE THE PLAYER
    public bool alert;                  // is the ghost alert right now? (does it still remember there's a player roaming the area?)

    // STUN PROPERTIES
    public bool stunned;
    public float stunTime;              // how long is the ghost stunned for?

    // VARIABLES NEEDED FOR CAMERA DETECTION FROM PLAYER
    public Collider objCollider;
    public Plane[] planes;
    public Camera cam;

    public bool playingSound1;
    public bool playingSound2;
    public bool playingSound3;
    public AudioSource audiosrc;
    public AudioClip breath1;
    public AudioClip breath2;
    public AudioClip breath3;
    public AudioClip whisper;

    void Start()
    {
        //cam = GameObject.Find("Player").GetComponent<CameraController>().currentCamera;
        planes = GeometryUtility.CalculateFrustumPlanes(GameServices.cameraController.currentCamera);
        player = GameObject.Find("Player");
        objCollider = GetComponent<Collider>();
        audiosrc.clip = whisper;
        //audiosrc.Play();
    }

    void Update()
    {
        if(cam == null)
        {
            cam = GameServices.cameraController.currentCamera;
        }

        if(!player)
        {
            player = GameServices.cameraController.gameObject;
        }

        if(Vector3.Distance(player.transform.position, transform.position) < 10f)
        {
            GameServices.gameCycleManager.BeingHunted2();
            Debug.Log(GameServices.gameCycleManager.sanityDrainRateGhost);
        }
        else if(Vector3.Distance(player.transform.position, transform.position) < 20f)
        {
            GameServices.gameCycleManager.BeingHunted1();
            Debug.Log(GameServices.gameCycleManager.sanityDrainRateGhost);
        }
        else
        {
            GameServices.gameCycleManager.ResetDrainRate();
            Debug.Log(GameServices.gameCycleManager.sanityDrainRateGhost);
        }

        // if the ghost is alert but doesn't see the player, start decrementing its memory so that it can eventually forget about the player
        if (!detected && currentDetectionTime > 0)
        {
            currentDetectionTime -= Time.deltaTime;
        }

        // camera business
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (Input.GetKey(KeyCode.F) && player.GetComponent<CameraController>().IsCameraReady() && GeometryUtility.TestPlanesAABB(planes, objCollider.bounds) && player.layer == 12)
        {
            // sending a raycast from ghost to player... if it hits, it stuns
            // we use a raycast to ensure that there is no wall or object between the ghost and the player
            Vector3 offset = new Vector3(0.0f, 1.3f, 0.0f); // this offset is to just match where the "eyes" of the ghost would be 
            RaycastHit hit;
            if (Physics.Linecast(transform.position + offset, player.transform.position, out hit))
            {
                if(hit.transform.tag == "Player")
                {
                    StartCoroutine(Stun());
                    stunned = true;
                }
            }
        }

        /*
        // keeping this comment cuz it might be useful for debugging later       
        else if(Input.GetKey(KeyCode.F) && player.GetComponent<CameraController>().IsCameraReady())
        {
            Debug.Log("Nothing has been detected");
        }*/

        // if stunned, stay in place and don't do anything else
        if (stunned)
        {
            playingSound1 = false;
            playingSound2 = false;
            playingSound3 = false;
            audiosrc.Stop();
            agent.SetDestination(transform.position);
            return;
        }

        // if it is not alert, just patrol
        if (!alert && backtracked)
        {
            if (!playingSound1 && player.layer == 12)
            {
                audiosrc.clip = whisper;
                audiosrc.Play();
                playingSound1 = true;
                playingSound2 = false;
                playingSound3 = false;
            }
            Patrol();
        }

        // if it is alert...
        else
        {
            // if it has forgotten about the player...
            if (currentDetectionTime <= 0)
            {
                // turn off alertedness if it has returned from a hunting detour
                if (backtracked)
                {
                    alert = false;
                }
                // keep backtracking back to its patrolling path
                else
                {
                    if (!playingSound2 && player.layer == 12)
                    {
                        audiosrc.clip = breath3;
                        audiosrc.Play();
                        playingSound1 = false;
                        playingSound2 = true;
                        playingSound3 = false;
                    }
                    BackTrack();
                }
            }
            // keep chasing the player
            else
            {
                if (!playingSound3 && player.layer == 12)
                {
                    audiosrc.clip = breath1;
                    audiosrc.Play();
                    playingSound1 = false;
                    playingSound2 = false;
                    playingSound3 = true;
                }
                backtracked = false;
                Hunt();
            }
        }
    }

    // stun function
    IEnumerator Stun()
    {
        yield return new WaitForSeconds(stunTime);
        stunned = false;
    }

    // function called by GhostVision script, which is a trigger that lets the ghost know if the player has appeared in the ghost's range of vision
    public void DetectPlayer()
    {
        RaycastHit hit;
        Vector3 offset = new Vector3(0.0f, 1.5f, 0.0f);
        if (Physics.Linecast(transform.position + offset, player.transform.position, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                // if it sees the player!
                detected = true;
                alert = true;
                currentDetectionTime = detectionTime;
            }
            else
            {
                detected = false;
            }
        }
    }

    // this is for when the player has left the ghost's field of vision
    public void UndetectPlayer()
    {
        detected = false;
    }

    // ghost cycles through spots array and patrols their area
    private void Patrol()
    {
        agent.SetDestination(spots[currentSpotNumber]);
        if(Vector3.Distance(this.transform.position, spots[currentSpotNumber]) < 2f)
        {
            currentSpotNumber++;
            if(currentSpotNumber == spots.Length)
            {
                currentSpotNumber = 0;
            }
        }
    }

    // ghost chases after player
    // also where the sanity drop would occur
    private void Hunt()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) > 5f)
        {
            Quaternion toRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * Time.deltaTime);
            agent.SetDestination(player.transform.position);
        }
        else if(Vector3.Distance(this.transform.position, player.transform.position) > 0.1f)
        {
            agent.SetDestination(this.transform.position);
        }
        if (Vector3.Distance(this.transform.position, player.transform.position) < 10f)
        {
             // DRASTICALLY DECREASE SANITY
        }
        else if (Vector3.Distance(this.transform.position, player.transform.position) < 20f)
        {
            // DECREASE SANITY A LITTLE LESS
        }
    }

    // ghost goes back to its patrolling path
    // when returning it actually skips to the next spot
    private void BackTrack()
    {
        if(Vector3.Distance(this.transform.position, spots[currentSpotNumber]) < 2f)
        {
            currentSpotNumber++;
            if (currentSpotNumber == spots.Length)
            {
                currentSpotNumber = 0;
            }
            backtracked = true;
        }
        agent.SetDestination(spots[currentSpotNumber]);
    }




    /* 
     * 
     * EVERYTHING BELOW IS FROM THE OLD PROTOTYPE OF THE GHOST
     * 
     * 
     */    





    /*private void RandomStartLocation()
    {
        transform.position = new Vector3(Random.Range(leftBound, rightBound), Random.Range(downBound, upBound), Random.Range(backBound, forwardBound));
    }*/
    /*private void Roam()
    {
        if(timeToTurn <= 0)
        {
            direction = Random.onUnitSphere;
            timeToTurn = Random.Range(70, 100);
        }
        if (transform.position.x < rightBound && transform.position.x > leftBound && transform.position.z < forwardBound && transform.position.z > backBound)
        {
            transform.position += new Vector3(direction.x, 0, direction.z) * speed * Time.deltaTime;
        }
        if (transform.position.x <= leftBound)
        {
            transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
        }
        if (transform.position.x >= rightBound)
        {
            transform.position += new Vector3(-1, 0, 0) * speed * Time.deltaTime;
        }/*
        if (transform.position.y <= downBound)
        {
            transform.position += new Vector3(0, 1, 0) * speed * Time.deltaTime;
        }
        if (transform.position.y >= upBound)
        {
            transform.position += new Vector3(0, -1, 0) * speed * Time.deltaTime;
        }*/
    /*
    if (transform.position.z <= backBound)
    {
        transform.position += new Vector3(0, 0, 1) * speed * Time.deltaTime;
    }
    if (transform.position.z >= forwardBound)
    {
        transform.position += new Vector3(0, 0, -1) * speed * Time.deltaTime;
    }
    Quaternion toRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 5 * Time.deltaTime);
    timeToTurn--;
}*/
}
