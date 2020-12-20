using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityController : MonoBehaviour
{

    Shader insanity;
    public GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        insanity = Shader.Find("Insanity");
        
    }

    // Update is called once per frame
    void Update()
    {
        //insanity.material.SetFloat("Opacity", GM.mentalHealth);
        Shader.SetGlobalFloat(Shader.PropertyToID("Opacity"), GM.mentalHealth);
    }
}
