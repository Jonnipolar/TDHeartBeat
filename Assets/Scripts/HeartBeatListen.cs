using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatListen : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        EventManager.StartListening("HeartBeat", HandleHeartBeat);
    }

    void HandleHeartBeat()
    {
        anim.SetTrigger("Beat");
    }
}
