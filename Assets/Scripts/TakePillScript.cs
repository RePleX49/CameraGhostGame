using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TakePillScript : MonoBehaviour , IPointerDownHandler
{
	public AudioSource audioSource;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (GameManager.me.pills > 0 &&
			GameManager.me.state == GameManager.me.game)
		{
			if(audioSource)
			{
				audioSource.Play();
			}
			
			GameManager.me.pills--;
			GameManager.me.mentalHealth -= GameManager.me.mHDecreaseAmount;
		}
	}
}
