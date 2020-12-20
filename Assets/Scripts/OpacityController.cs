using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityController : MonoBehaviour
{

    //Shader insanity;
    public GameManager GM;
    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        //insanity = Shader.Find("Insanity");
        rend = GetComponent<Renderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        rend.material.SetFloat("_Opacity", GM.mentalHealth);
        //Shader.SetGlobalFloat(Shader.PropertyToID("Opacity"), GM.mentalHealth);
    }
}
