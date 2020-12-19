﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public List<GameObject> objectsToChange;
	public bool lamp;
	public bool door;

    //Added by Raymond 12-17-20
    public bool lid;

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


	private bool clockWise = true;
	private float spinSpd;
	private float actualSpinSpd;

	private void Start()
	{
		lampFlashIntervalMin = 0.3f;
		lampFlashIntervalMax = 1f;
		doorMoveIntervalMin = 3f;
		doorMoveIntervalMin = 1f;

        lidMoveIntervalMin = 3f;
        lidMoveIntervalMax = 1f;


		spinSpd = 500f;
        flapSpd = 500f;

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
					}
					else
					{
						objectsToChange[0].SetActive(true);
					}	
				}
			}
			else if (door)
			{
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
					else // open and close
					{

					}
				}
			}
            //Added by Raymond 12-17-20
            else if (lid)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                    objectsToChange[0].transform.Rotate(0, 0, actualFlapSpd * Time.deltaTime);
                }
                else
                {
                    timer = Random.Range(lidMoveIntervalMin, lidMoveIntervalMax);
                    if(objectsToChange[0].GetComponent<FlapScript>().flap)// flap
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
                    else
                    {

                    }

                }
            }
		}
	}

	// Getters

	public Vector3 GetPosition()
	{
		return transform.position;
	}
}
