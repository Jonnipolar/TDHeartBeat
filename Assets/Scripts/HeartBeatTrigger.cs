using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeartBeatTrigger : MonoBehaviour
{    
    public float startDelay = 0.0f;
    public float beatInterval = 1.0f;

    void Start()
    {
        StartCoroutine(BeatCoroutine());
    }

    IEnumerator BeatCoroutine()
    {
        yield return new WaitForSeconds(startDelay);
        while(true)
        {
            yield return new WaitForSeconds(beatInterval);
            EventManager.TriggerEvent("HeartBeat");
        }
    }
    
}
