using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyFoodScript : MonoBehaviour , IPointerDownHandler
{
	public AudioSource audioSource;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (GameManager.me.money >= GameManager.me.foodPrice &&
			GameManager.me.foodNum < 9 &&
			GameManager.me.state == GameManager.me.game)
		{
			if(audioSource)
			{
				audioSource.Play();
			}

			GameManager.me.money -= GameManager.me.foodPrice;
			GameManager.me.foodNum++;
		}
	}
}
