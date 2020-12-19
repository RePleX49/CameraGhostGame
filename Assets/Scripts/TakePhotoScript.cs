using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TakePhotoScript : MonoBehaviour , IPointerDownHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		if (GameManager.me.filmNum > 0 && GameManager.me.state == GameManager.me.game)
			InGameCamScript.me.state = InGameCamScript.me.showCamAim;
	}
}
