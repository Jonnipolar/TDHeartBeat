using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusMovement : MonoBehaviour
{
    public float movementTime = 0.5f;
    public Stack<Vector2> path;

    float inverseMovementTime;
    Rigidbody2D rb;
    bool moving;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inverseMovementTime = 1.0f / movementTime;
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
