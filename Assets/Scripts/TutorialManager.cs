using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private int state = 0;
	public List<GameObject> tuts;
	public int index = 0;

	private void Update()
	{
		if (GameManager.me.state == GameManager.me.tutorial)
		{
			tuts[index].SetActive(true);
			if (Input.GetMouseButtonDown(0))
			{
				//if (index != 0)
				{
					tuts[index].SetActive(false);
				}
				if (index < tuts.Count - 1)
				{
					index++;
				}
				else
				{
					GameManager.me.state = GameManager.me.game;
				}
			}
		}
	}
}
