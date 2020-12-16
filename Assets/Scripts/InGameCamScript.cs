using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCamScript : MonoBehaviour
{
    public static InGameCamScript me;
    public float speed = 0f;
    public GameObject photo;
    public GameObject camAim;
    public GameObject ghost;
    public GameObject coverPrefab;
    public bool hidePhoto;
    public int state = 0;
    public int idle = 0;
    public int showCamAim = 1;
    public int showPhoto = 2;
    public bool ghostCaptured = false;

    float initialCamHeight = 0.0f;
    public float screenDepthOffset = 10.0f;
    float initialMouseZ;

	private void Start()
	{
        me = this;
        state = idle;
        initialCamHeight = transform.position.y;
        initialMouseZ = Camera.main.WorldToScreenPoint(transform.position).z;
	}

	private void Update()
	{
        if (state == idle)
		{
            hidePhoto = false;
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            camAim.SetActive(false);
		}
        else if (state == showCamAim)
		{
            camAim.SetActive(true);

            Vector3 mousePosScreen = Input.mousePosition;
            mousePosScreen.z = initialMouseZ;
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);
            mousePosWorld = new Vector3(mousePosWorld.x, initialCamHeight, mousePosWorld.z);

            transform.position = mousePosWorld;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                int ghostCount = 0;
                GhostScript[] ghosts = FindObjectsOfType<GhostScript>();

                foreach(GhostScript ghost in ghosts)
                {
                    if(ghost.gameObject.GetComponent<SpriteRenderer>().isVisible)
                    {
                        Debug.Log("Captured Ghost");
                        ghostCount++;
                        GameManager.me.money += 100;
                    }

                    //TODO implement formula to calculate total money
                }

                GameManager.me.filmNum--;
                photo.SetActive(true);

                GameObject exposureFX = Instantiate(coverPrefab);
                Vector3 camPosition = new Vector3(Camera.main.transform.position.x, 
                    photo.transform.position.y, photo.transform.position.z);

                photo.transform.position = camPosition;
                exposureFX.transform.position = camPosition;
                state = showPhoto;
            }
        }
        else if (state == showPhoto)
		{
            if (hidePhoto)
			{            
                photo.SetActive(false);
                camAim.SetActive(false);
                state = idle;
			}
		}
    }
}
