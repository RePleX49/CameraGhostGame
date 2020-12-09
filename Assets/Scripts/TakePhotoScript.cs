using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhotoScript : MonoBehaviour
{
	private void OnMouseDown()
	{
		if (GameManager.me.filmNum > 0 && GameManager.me.state == GameManager.me.game)
			InGameCamScript.me.state = InGameCamScript.me.showCamAim;
	}
}
