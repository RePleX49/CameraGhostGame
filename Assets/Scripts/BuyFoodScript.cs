using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyFoodScript : MonoBehaviour
{
	private void OnMouseDown()
	{
		if (GameManager.me.money >= GameManager.me.foodPrice &&
			GameManager.me.foodNum < 9 &&
			GameManager.me.state == GameManager.me.game)
		{
			GameManager.me.money -= GameManager.me.foodPrice;
			GameManager.me.foodNum++;
		}
	}
}
