using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    //public float leftBound, rightBound, downBound, upBound, backBound, forwardBound;
    public Vector3[] spots;
    public int currentSpotNumber;

    public List<Vector3> backtrack = new List<Vector3>();
    public bool backtracked = true;

    //public int timeToTurn;
    //public Vector3 direction;
    public float speed;

    public GameObject player;
    //public float distanceFromPlayer;
    //public float detectionDistance;
    public bool detected;
    public float detectionTime;         // the maximum amount of time the ghost has until forgetting about the player
    public float currentDetectionTime;  // the amount of time the ghost currently has until forgetting about the player
    public bool alert;

    public bool stunned;

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
        /*if(currentDetectionTime <= 0)
        {
            alert = false;
        }*/
        planes = GeometryUtility.CalculateFrustumPlanes(cam);

        Debug.Log("" + player.GetComponent<CameraController>().IsCameraReady());
        if (Input.GetKey(KeyCode.F) && player.GetComponent<CameraController>().IsCameraReady() && GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        {
            Debug.Log("hi");
            RaycastHit hit;
            if (Physics.Linecast(transform.position, player.transform.position, out hit))
            {
                if(hit.transform.tag == "Player")
                {
                    StartCoroutine(Stun());
                    stunned = true;
                }
            }
        }
        else if(Input.GetKey(KeyCode.F) && player.GetComponent<CameraController>().IsCameraReady())
        {
            Debug.Log("Nothing has been detected");
        }
        if (stunned)
        {
            return;
        }

        if (!alert && backtracked)
        {
            Patrol();
        }
        else
        {
            if (detected)
            {
                backtracked = false;
                Hunt();
            }
            else
            {
                if(currentDetectionTime <= 0)
                {
                    if (backtracked)
                    {
                        //Debug.Log("how?");
                        alert = false;
                    }
                    else
                    {
                        BackTrack();
                    }
                }
            }
        }
        //distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        /*if(distanceFromPlayer < detectionDistance)
        {
            //Hunt();
        }
        else
        {
            //Roam();
        }*/
    }

    IEnumerator Stun()
    {
        yield return new WaitForSeconds(5f);
        stunned = false;
    }

    public void DetectPlayer()
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position, player.transform.position, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                //Debug.Log("detected!");
                detected = true;
                alert = true;
                currentDetectionTime = detectionTime;
            }
            else
            {
                /*Debug.Log("Name: " + hit.transform.gameObject.name);
                Debug.Log("Tag: " + hit.transform.tag);*/
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
        Quaternion toRotation = Quaternion.LookRotation(spots[currentSpotNumber] - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
        //Debug.Log("" + Vector3.Distance(this.transform.position, spots[currentSpotNumber]));
        if(Vector3.Distance(this.transform.position, spots[currentSpotNumber]) < 0.6f)
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
        if (Vector3.Distance(this.transform.position, player.transform.position) > 0.6f)
        {
            Quaternion toRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
            backtrack.Add(this.transform.position);
        }
    }

    private void BackTrack()
    {
        Debug.Log("backtracking");
        if(backtrack.Count == 0)
        {
            Debug.Log("Hm" + backtrack.Count);
            backtracked = true;
        }
        transform.position = backtrack[backtrack.Count - 1];
        backtrack.RemoveAt(backtrack.Count - 1);
    }

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
