using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyFilmScript : MonoBehaviour, IPointerDownHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		if (GameManager.me.money >= GameManager.me.flimPrice &&
			GameManager.me.filmNum < 9 &&
			GameManager.me.state == GameManager.me.game)
		{
			GameManager.me.money -= GameManager.me.flimPrice;
			GameManager.me.filmNum += 1;
		}
	}
}
