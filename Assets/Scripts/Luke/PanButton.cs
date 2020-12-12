using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PanButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    CameraPanner panner;

    // Start is called before the first frame update
    void Start()
    {
        panner = Camera.main.GetComponent<CameraPanner>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Entered selecteable");

        if (gameObject.CompareTag("LeftHover"))
        {
            panner.SetPanLeft(true);
        }
        else if (gameObject.CompareTag("RightHover"))
        {
            panner.SetPanRight(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            if (gameObject.CompareTag("LeftHover"))
            {
                panner.SetPanLeft(false);
            }
            else if (gameObject.CompareTag("RightHover"))
            {
                panner.SetPanRight(false);
            }
    }
}
