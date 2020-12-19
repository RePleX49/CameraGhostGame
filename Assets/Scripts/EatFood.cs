using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EatFood : MonoBehaviour , IPointerDownHandler
{
	public AudioSource audioSource;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (GameManager.me.foodNum >= 0 && GameManager.me.state == GameManager.me.game)
		{
			if(audioSource)
			{
				audioSource.Play();
			}

			GameManager.me.foodNum--;
			GameManager.me.physicalHealth += GameManager.me.pHIncreaseAmount;
			GameManager.me.physicalHealth = Mathf.Min(GameManager.me.physicalHealth, 100);
		}
	}
}
