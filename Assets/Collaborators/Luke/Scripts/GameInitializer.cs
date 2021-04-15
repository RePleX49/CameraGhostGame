using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public int alternateSceneBuildIndex = 1;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(PlayerPrefab, transform.position, transform.rotation);
        SceneManager.LoadScene(alternateSceneBuildIndex, LoadSceneMode.Additive);       
    }
}
