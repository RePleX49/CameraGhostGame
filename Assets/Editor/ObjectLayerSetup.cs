using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectLayerSetup : EditorWindow
{
    int layerDropdownIndex = 0;
    static List<string> layerNames = new List<string>();

    [MenuItem("Tools/Set All Object Layer To")]
    private static void ShowWindow()
    {
        for(int i = 8; i <= 31; i++)
        {
            string name = LayerMask.LayerToName(i);
            if(name.Length > 0)
            {
                layerNames.Add(name);
            }
        }
        GetWindow(typeof(ObjectLayerSetup));
    }

    [MenuItem("Tools/Teleport to Level End")]
    private static void GetToLevelEnd()
    {
        GameServices.cameraController.gameObject.transform.position = GameObject.Find("LevelEnd").transform.position;
        Debug.Log("Should teleport");
    }

    private void OnGUI()
    {
        GUILayout.Label("Set Layer Of Scene Objects", EditorStyles.boldLabel);

        layerDropdownIndex = EditorGUILayout.Popup(layerDropdownIndex, layerNames.ToArray());

        if (GUILayout.Button("Set Layer"))
        {
            SetLayers();
        }
        
    }

    private void SetLayers()
    {
        GameObject[] sceneObjects = FindObjectsOfType<GameObject>();

        foreach(GameObject gameObject in sceneObjects)
        {
            gameObject.layer = LayerMask.NameToLayer(layerNames[layerDropdownIndex]);
        }
    }
}
