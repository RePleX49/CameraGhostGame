using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteCoverScript : MonoBehaviour
{
    public float spd = .1f;
    private float timer = 0;
    public float photoShowTime = 0;

	private void Start()
	{
        timer = photoShowTime;
	}

	void Update()
    {
        float alpha = GetComponent<MeshRenderer>().material.color.a;
        
        GetComponent<MeshRenderer>().material.color = new Color(1f, 1f, 1f, Mathf.Clamp(alpha - spd * Time.deltaTime,0,1));
        if (alpha <= 0)
		{
            if (timer > 0)
			{
                timer -= Time.deltaTime;
			}
			else
			{
                InGameCamScript.me.hidePhoto = true;
                Destroy(gameObject);
            }
		}
    }
}
