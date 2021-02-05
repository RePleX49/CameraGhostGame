using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SleepScript : MonoBehaviour , IPointerDownHandler
{
	public AudioSource audioSource;
	public int healthCost = 10;
	public int mentalRegen = 20;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (GameManager.me.state == GameManager.me.game)
		{
			if(audioSource)
			{
				audioSource.Play();
			}
			
			GameManager.me.mentalHealth += mentalRegen;
			GameManager.me.physicalHealth -= healthCost;
			GameManager.me.gR += GameManager.me.grIncreaseAmount;
		}
	}
}
