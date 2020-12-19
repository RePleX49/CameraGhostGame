using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyPillScript : MonoBehaviour , IPointerDownHandler
{
	public AudioSource audioSource;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (GameManager.me.money >= GameManager.me.pillPrice &&
			GameManager.me.pills < 9 &&
			GameManager.me.state == GameManager.me.game)
		{
			if(audioSource)
			{
				audioSource.Play();
			}

			GameManager.me.money -= GameManager.me.pillPrice;
			GameManager.me.pills += 1;
		}
	}
}
