using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePillScript : MonoBehaviour
{
	private void OnMouseDown()
	{
		if (GameManager.me.pills > 0 &&
			GameManager.me.state == GameManager.me.game)
		{
			GameManager.me.pills--;
			GameManager.me.mentalHealth -= GameManager.me.mHDecreaseAmount;
		}
	}
}
