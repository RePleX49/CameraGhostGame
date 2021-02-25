using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotScript : MonoBehaviour
{
    RenderTexture snapshotTex;
    Texture2D snapshotTexture2D;
    Material targetMaterial;
    public int renderDepthResolution = 16;
    public Image snapshotImage;
    public Image exposureImage;

    public float exposureDuration = 1.0f;

    bool isTakingPicture;

    // Start is called before the first frame update
    void Start()
    {
        snapshotTex = new RenderTexture(Screen.width, Screen.height, renderDepthResolution);
        snapshotTexture2D = new Texture2D(Screen.width, Screen.height);
        targetMaterial = new Material(Shader.Find("Unlit/GreyScale"));
        snapshotImage.material = targetMaterial;
        isTakingPicture = false;

        DisableImages();
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
        if (isTakingPicture)
            return;

        Camera altCamera = GameServices.cameraController.altCamera;
        //Graphics.Blit(altCamera.targetTexture, snapshotTex);
        RenderTexture.active = altCamera.targetTexture;
        snapshotTexture2D.ReadPixels(new Rect(0, 0, altCamera.targetTexture.width, altCamera.targetTexture.height), 0, 0);
        snapshotTexture2D.Apply();
        targetMaterial.mainTexture = (Texture)snapshotTexture2D;

        exposureImage.enabled = true;
        snapshotImage.enabled = true;

        StartCoroutine(PlaySnapshotEffect());
    }

    IEnumerator PlaySnapshotEffect()
    {
        float elapsedTime = 0.0f;
        isTakingPicture = true;
        GameServices.cameraController.DisableCamera();

        while(elapsedTime < exposureDuration)
        {
            float alpha;
            if (elapsedTime > 1.0f)
            {
                alpha = 0f;
            }
            else
            {
                alpha = (1 - GetParametricFunction(elapsedTime));
            }

            Color newColor = new Color(exposureImage.color.r, exposureImage.color.g, exposureImage.color.b, alpha);
            exposureImage.color = newColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        DisableImages();
        isTakingPicture = false;
        GameServices.cameraController.EnableCamera();

        yield return null;
    }

    float GetParametricFunction(float time)
    {
        float sqt = time * time;
        return (sqt / (2.0f * (sqt - time) + 1.0f));
    }

    void DisableImages()
    {
        snapshotImage.enabled = false;
        exposureImage.enabled = false;
    }
}
