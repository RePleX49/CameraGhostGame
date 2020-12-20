using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public List<GameObject> objectsToChange;
	public bool lamp;
	public bool door;
    public bool shower;

    //Added by Raymond 12-17-20
    public bool lid;
    public bool levitate;

    public bool effected = false;
	private float timer;
	private float lampFlashIntervalMin;
	private float lampFlashIntervalMax;
	private float doorMoveIntervalMin;
	private float doorMoveIntervalMax;
    
    //Added by Raymond 12-17-20 
    private float lidFlapMin;
    private float lidFlapMax;
    private float flapSpd;
    private float actualFlapSpd;
    private float lidMoveIntervalMin;
    private float lidMoveIntervalMax;
    private float initY; // to add the output of a SinWave
    public float levitateHght;

    public bool toilet;
    public bool stove;

	private bool clockWise = true;
	private float spinSpd;
	private float actualSpinSpd;

    bool audioPlaying = false;

    ObjectAudio audioScript;

	private void Start()
	{
        audioScript = GetComponent<ObjectAudio>();

		lampFlashIntervalMin = 0.3f;
		lampFlashIntervalMax = 1f;
		doorMoveIntervalMin = 3f;
		doorMoveIntervalMin = 1f;

        lidMoveIntervalMin = 3f;
        lidMoveIntervalMax = 1f;


		spinSpd = 500f;
        flapSpd = 500f;

        initY = transform.position.y;

		if (lamp)
		{
			timer = Random.Range(lampFlashIntervalMin, lampFlashIntervalMax);
		}
		else if (door)
		{
			timer = Random.Range(doorMoveIntervalMin, doorMoveIntervalMax);
		}

        else if (lid)
        {
            timer = Random.Range(lidMoveIntervalMin, lidMoveIntervalMax);
        }
	}

	private void Update()
	{
		if (effected)
		{
            if (lamp)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    timer = Random.Range(lampFlashIntervalMin, lampFlashIntervalMax);
                    if (objectsToChange[0].activeSelf)
                    {
                        objectsToChange[0].SetActive(false);
                        audioScript.PlayStart();
                    }
                    else
                    {
                        objectsToChange[0].SetActive(true);
                        audioScript.PlayEnd();
                    }
                }
            }
            else if (door)
            {
                if(!audioPlaying)
                {
                    audioScript.PlayStart();
                    audioScript.PlayConstant();
                    audioPlaying = true;
                }

                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    objectsToChange[0].transform.Rotate(0, actualSpinSpd * Time.deltaTime, 0);
                }
                else
                {
                    timer = Random.Range(doorMoveIntervalMin, doorMoveIntervalMax);
                    if (objectsToChange[0].GetComponent<SpinScript>().spin) // spin
                    {
                        if (clockWise)
                        {
                            actualSpinSpd = -spinSpd;
                            clockWise = false;
                        }
                        else
                        {
                            actualSpinSpd = spinSpd;
                            clockWise = true;
                        }                       
                    }
                }
            }
            //Added by Raymond 12-17-20
            else if (lid)
            {
                if(!audioPlaying)
                {
                    audioScript.PlayStart();
                    audioScript.PlayConstant();
                    audioPlaying = true;
                }

                if (stove == true)
                {
                    if (timer > 0)
                    {
                        timer -= Time.deltaTime;
                        objectsToChange[0].transform.Rotate(actualFlapSpd * Time.deltaTime, 0, 0);
                    }
                    else
                    {
                        timer = Random.Range(lidMoveIntervalMin, lidMoveIntervalMax);
                        if (objectsToChange[0].GetComponent<FlapScript>().flap)// flap
                        {
                            if (clockWise)
                            {
                                actualSpinSpd = -flapSpd;
                                clockWise = false;
                            }
                            else
                            {
                                actualFlapSpd = flapSpd;
                                clockWise = true;
                            }                          
                        }
                    }
                }

                if (toilet == true)
                {
                    if (timer > 0)
                    {
                        timer -= Time.deltaTime;
                        objectsToChange[0].transform.Rotate(0, 0, actualFlapSpd * Time.deltaTime);
                    }
                    else
                    {
                        timer = Random.Range(lidMoveIntervalMin, lidMoveIntervalMax);
                        if (objectsToChange[0].GetComponent<FlapScript>().flap)// flap
                        {
                            if (clockWise)
                            {
                                actualSpinSpd = -flapSpd;
                                clockWise = false;
                            }
                            else
                            {
                                actualFlapSpd = flapSpd;
                                clockWise = true;
                            }
                        }
                    }
                }

            }
            else if (levitate)
            {
                if(!audioPlaying)
                {
                    audioScript.PlayStart();
                    audioScript.PlayConstant();
                    audioPlaying = true;
                }

                //objectsToChange[0].SetActive(true);
                foreach (GameObject floaty in objectsToChange)
                {
                    floaty.SetActive(true);
                }

                if(shower)
                {
                    return;
                }

                Debug.Log("toasty!");
                float newY = initY + (levitateHght * Mathf.Sin(Time.time));
                Vector3 newPosition = new Vector3(transform.position.x, newY, transform.position.z);
                transform.position = newPosition;
            }
		}
        else
        {
            if (levitate == true)
            {
                foreach (GameObject floaty in objectsToChange)
                {
                    floaty.SetActive(false);
                }
            }

            audioScript.StopConstant();
            audioPlaying = false;
        }
	}

	// Getters

	public Vector3 GetPosition()
	{
		return transform.position;
	}
}
