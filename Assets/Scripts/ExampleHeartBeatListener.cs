using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleHeartBeatListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("HeartBeat", HandleHeartBeat);
    }

    void HandleHeartBeat()
    {
        Debug.Log("HeartBeat");
    }
}
