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
	ObjectScript currentHaunt;
	GhostManager ghostManager;

	private void Start()
	{
		showInterval = initialInterval;
		timer = showInterval;

		// assign a random sprite when spawned
        int rn = Random.Range(0, sprites.Count);
        GetComponent<SpriteRenderer>().sprite = sprites[rn];
    }

	public void Initialize(GhostManager manager, ObjectScript newHaunt)
	{
		ghostManager = manager;

        // set our current haunt and position to haunt position
        currentHaunt = newHaunt;

        if (currentHaunt)
        {
            transform.position = currentHaunt.GetPosition();

            // set haunt to be effected
            currentHaunt.effected = true;
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

	public ObjectScript GetHaunt()
	{
		return currentHaunt;
	}

	public void ResetHaunt()
	{
		if(currentHaunt)
		{
			currentHaunt.effected = false;
		}
	}

	public void Captured()
	{
		if(ghostManager)
		{
			ghostManager.ClearGhost(gameObject, currentHaunt);
			ResetHaunt();
			Destroy(this.gameObject);
		}
	}
}
