using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Virus : MonoBehaviour
{
    public float movementTime = 0.5f;
    public float health = 100f;
    // [HideInInspector]
    public float currentMovementTime = 0.5f;

    private void Awake() 
    {
        currentMovementTime = movementTime;
    }
    public void Slow(float pct)
    {
        currentMovementTime = movementTime * (1f + pct);
        StartCoroutine(SlowCoroutine());
    }

    IEnumerator SlowCoroutine()
    {
        yield return new WaitForSeconds(1);
        currentMovementTime = movementTime;
    }
}
