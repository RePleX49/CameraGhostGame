using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraPanner : MonoBehaviour
{
    public float maxDistRight;
    public float maxDistLeft;

    public float panRate;

    Vector3 initialPos;

    public GameObject cameraObject;

    bool bPanLeft = false;
    bool bPanRight = false;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = cameraObject.transform.localPosition;
    }

    private void Update()
    {
        if(bPanLeft)
        {
            PanLeft();
        }
        else if(bPanRight)
        {
            PanRight();
        }
    }

    void PanRight()
    {
        if(cameraObject.transform.localPosition.x < (initialPos.x + maxDistRight))
        {
            Vector3 newPos = new Vector3(panRate * Time.deltaTime, 0f, 0f);
            cameraObject.transform.localPosition += newPos;
        }
    }

    void PanLeft()
    {
        if (cameraObject.transform.localPosition.x > (initialPos.x - maxDistLeft))
        {
            Vector3 newPos = new Vector3(panRate * Time.deltaTime, 0f, 0f);
            cameraObject.transform.localPosition -= newPos;
        }
    }

    public void SetPanLeft(bool val)
    {
        bPanLeft = val;
    }

    public void SetPanRight(bool val)
    {
        bPanRight = val;
    }
}
