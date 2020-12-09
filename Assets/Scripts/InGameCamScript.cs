using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

	private void Start()
	{
        me = this;
        state = idle;
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
            float xPos = transform.position.x + Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed;
            Mathf.Clamp(xPos, -10.67f, 10.73f);
            if (xPos > -10.76 && xPos < 10.73)
            {
                transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameManager.me.filmNum--;
                photo.SetActive(true);
                Instantiate(coverPrefab);
                state = showPhoto;
            }
        }
        else if (state == showPhoto)
		{
            if (hidePhoto)
			{
                GameManager.me.money += 100;
                photo.SetActive(false);
                camAim.SetActive(false);
                state = idle;
			}
		}
    }
}
