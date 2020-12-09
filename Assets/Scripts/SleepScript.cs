using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepScript : MonoBehaviour
{
	private void OnMouseDown()
	{
		if (GameManager.me.state == GameManager.me.game)
		{
			GameManager.me.mentalHealth += GameManager.me.mHIncreaseAmount;
			GameManager.me.physicalHealth -= GameManager.me.pHDecreaseAmount;
			GameManager.me.gR += GameManager.me.grIncreaseAmount;
		}
	}
}
