using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPillScript : MonoBehaviour
{
	private void OnMouseDown()
	{
		if (GameManager.me.money >= GameManager.me.pillPrice &&
			GameManager.me.pills < 9 &&
			GameManager.me.state == GameManager.me.game)
		{
			GameManager.me.money -= GameManager.me.pillPrice;
			GameManager.me.pills += 1;
		}
	}
}
