using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    public float leftBound, rightBound, downBound, upBound, backBound, forwardBound;

    // time until ghost will turn
    // will likely be randomly generated everytime
    public int timeToTurn;
    public Vector3 direction;
    public float speed;

    public GameObject player;
    public float distanceFromPlayer;
    public float detectionDistance;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        RandomStartLocation();
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        if(distanceFromPlayer < detectionDistance)
        {
            Hunt();
        }
        else
        {
            Roam();
        }
    }

    private void RandomStartLocation()
    {
        transform.position = new Vector3(Random.Range(leftBound, rightBound), Random.Range(downBound, upBound), Random.Range(backBound, forwardBound));
    }

    private void Roam()
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
    }

    private void Hunt()
    {
        Quaternion toRotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10 * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
