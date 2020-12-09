using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
	public List<Vector3> posList;
	public List<Sprite> sprites;
	public List<GameObject> objects;
	private float showInterval = 0;
	private float showDuration = 0;
	public float initialInterval = 20;
	public float initialDuration = 3;
	private float timer = 0;
	private bool moveObjectSwitch = false;

	private void Start()
	{
		showInterval = initialInterval;
		timer = showInterval;
	}

	private void Update()
	{
		if (GameManager.me.state == GameManager.me.game)
		{
			showInterval = initialInterval * GameManager.me.mentalHealth / 100f; // relationship determines how long it appears, mental health determines how frequently it appears
			showDuration = initialDuration + GameManager.me.gR / 33.33f;
			if (timer > 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				int rn = Random.Range(0, sprites.Count);
				GetComponent<SpriteRenderer>().sprite = sprites[rn];
				int rn2 = Random.Range(0, objects.Count);
				if (!moveObjectSwitch)
				{
					objects[rn2].GetComponent<ObjectScript>().effected = true;
					transform.position = posList[rn2];
					moveObjectSwitch = true;
					timer = showInterval;
				}
				else
				{
					foreach (GameObject obj in objects)
					{
						obj.GetComponent<ObjectScript>().effected = false;
						timer = showInterval;
					}
					moveObjectSwitch = false;
				}
			}
		}
	}

	void OnBecameInvisible()
    {
		InGameCamScript.me.ghostCaptured = false;
    }

	private void OnBecameVisible()
	{
		InGameCamScript.me.ghostCaptured = true;  
	}
}
