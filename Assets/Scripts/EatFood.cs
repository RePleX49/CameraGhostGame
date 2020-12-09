using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatFood : MonoBehaviour
{
	private void OnMouseDown()
	{
		if (GameManager.me.foodNum >= 0 && GameManager.me.state == GameManager.me.game)
		{
			GameManager.me.foodNum--;
			GameManager.me.physicalHealth += GameManager.me.pHIncreaseAmount;
			GameManager.me.physicalHealth = Mathf.Min(GameManager.me.physicalHealth, 100);
		}
	}
}
