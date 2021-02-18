using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotScript : MonoBehaviour
{
    RenderTexture snapshotTex;

    public GameObject sourceMatObject;
    public GameObject targetMatObject;

    Material targetMaterial;

    public int renderDepthResolution = 16;

    // Start is called before the first frame update
    void Start()
    {
        snapshotTex = new RenderTexture(Screen.width, Screen.height, renderDepthResolution);
        targetMaterial = targetMatObject.GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            TakeSnapshot();
        }
    }

    void TakeSnapshot()
    {
        Camera altCamera = GameServices.cameraController.altCamera;
        Graphics.Blit(altCamera.targetTexture, snapshotTex);
        targetMaterial.mainTexture = snapshotTex;
    }
}
