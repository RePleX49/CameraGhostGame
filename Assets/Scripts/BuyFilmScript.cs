using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyFilmScript : MonoBehaviour
{
	private void OnMouseDown()
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
