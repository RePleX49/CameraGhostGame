using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializationTest : MonoBehaviour
{
    public interface ISaveable
    {
        int Version { get; }
        string GetString();
        object SetFromString(string textData);
        int NumberOfBytes();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
