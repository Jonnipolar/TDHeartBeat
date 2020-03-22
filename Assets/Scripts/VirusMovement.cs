using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Virus))]
public class VirusMovement : MonoBehaviour
{
    public Stack<Vector2> path;

    float inverseMovementTime;
    Rigidbody2D rb;
    bool moving;
    private Virus virus;
    private bool movementChange = false;
    
    void Start()
    {
        virus = GetComponent<Virus>();
        rb = GetComponent<Rigidbody2D>();
        inverseMovementTime = 1.0f / virus.currentMovementTime;
    }

    void followPath()
    {
        if(!moving && path.Count > 0)
        {
            MoveTo(path.Pop());
        }
    }

    void MoveTo(Vector3 pos)
    {
        StartCoroutine(MovementCoroutine(pos));
    }

    IEnumerator MovementCoroutine(Vector3 endPos)
    {
        moving = true;

        float sqrDist = (transform.position - endPos).sqrMagnitude;
        while(sqrDist > float.Epsilon)
        {
            Vector3 newPostion = Vector2.MoveTowards(rb.position, endPos, inverseMovementTime * Time.deltaTime);

            rb.MovePosition(newPostion);

            sqrDist = (transform.position - endPos).sqrMagnitude;

            yield return null;
        }

        moving = false;
    }

    void Update() 
    {
        if(virus.currentMovementTime != virus.movementTime)
        {
            movementChange = true;
            inverseMovementTime = 1.0f / virus.currentMovementTime;
        }

        if(virus.currentMovementTime == virus.movementTime && movementChange)
        {
            movementChange = false;
            inverseMovementTime = 1.0f / virus.currentMovementTime;
        }

        if(path != null)
        {
            followPath();    
        }
    }


    public void SetPath(Stack<Vector2> newPath) 
    {
        path = newPath;
    }

}
