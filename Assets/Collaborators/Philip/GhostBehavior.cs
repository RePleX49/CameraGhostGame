using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostBehavior : MonoBehaviour
{
    // NECESSARY COMPONENT FOR PATH FINDING
    public NavMeshAgent agent;

    // WE USE A VECTOR3 ARRAY TO STORE AND CYCLE THROUGH DIFFERENT
    public Vector3[] spots;
    public int currentSpotNumber;
    public bool backtracked = true;

    public GameObject player;
    public bool detected;
    public float detectionTime;         // the maximum amount of time the ghost has until forgetting about the player
    public float currentDetectionTime;  // the amount of time the ghost currently has until forgetting about the player
    public bool alert;

    public bool stunned;
    public float stunTime;

    public Collider objCollider;
    public Plane[] planes;
    public Camera cam;

    void Start()
    {
        cam = GameObject.Find("Player").GetComponent<CameraController>().currentCamera;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        player = GameObject.Find("Player");
        objCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if(!detected && currentDetectionTime > 0)
        {
            currentDetectionTime -= Time.deltaTime;
        }
        planes = GeometryUtility.CalculateFrustumPlanes(cam);

        if (Input.GetKey(KeyCode.F) && player.GetComponent<CameraController>().IsCameraReady() && GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        {
            Vector3 offset = new Vector3(0.0f, 1.3f, 0.0f);
            RaycastHit hit;
            if (Physics.Linecast(transform.position + offset, player.transform.position, out hit))
            {
                if(hit.transform.tag == "Player")
                {
                    StartCoroutine(Stun());
                    stunned = true;
                }
            }
        }/*
        else if(Input.GetKey(KeyCode.F) && player.GetComponent<CameraController>().IsCameraReady())
        {
            Debug.Log("Nothing has been detected");
        }*/
        if (stunned)
        {
            agent.SetDestination(transform.position);
            return;
        }

        if (!alert && backtracked)
        {
            Patrol();
        }
        else
        {
            if (currentDetectionTime <= 0)
            {
                if (backtracked)
                {
                    alert = false;
                }
                else
                {
                    BackTrack();
                }
            }
            else
            {
                backtracked = false;
                Hunt();
            }
        }
    }

    IEnumerator Stun()
    {
        yield return new WaitForSeconds(stunTime);
        stunned = false;
    }

    public void DetectPlayer()
    {
        RaycastHit hit;
        Vector3 offset = new Vector3(0.0f, 1.3f, 0.0f);
        if (Physics.Linecast(transform.position + offset, player.transform.position, out hit))
        {
            Debug.Log("hello");
            Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Player")
            {
                Debug.Log("hi");
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

    public void UndetectPlayer()
    {
        detected = false;
    }

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
