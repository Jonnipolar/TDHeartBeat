using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusMovement : MonoBehaviour
{
    public float movementTime = 0.5f;
    public Stack<Vector2> path;

    float inverseMovementTime;
    Rigidbody2D rb;
    Animator animator;
    bool moving;

    void Start()
    {
        path = new Stack<Vector2>();
        path.Push(new Vector2(-4, 1));
        path.Push(new Vector2(2, 2));
        path.Push(new Vector2(1, 1));
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        inverseMovementTime = 1.0f / movementTime;
    }

    void followPath()
    {
        if(!moving)
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
        Animate(endPos - (Vector3)rb.position);
        while(sqrDist > 0.01f)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, endPos, inverseMovementTime * Time.deltaTime);

            rb.MovePosition(newPosition);

            sqrDist = (transform.position - endPos).sqrMagnitude;

            yield return null;
        }

        moving = false;
    }

    void Animate(Vector3 move)
    {
        animator.SetFloat("Horizontal", move.x);
		animator.SetFloat("Vertical", move.y);
		animator.SetFloat("Magnitude", move.magnitude);
    }

    void Update() 
    {
        if(path != null && path.Count > 0)
        {
            followPath();    
        }
        else {
            Animate(new Vector3(0, 0, 0));
        }
    }

    public void SetPath(Stack<Vector2> newPath) 
    {
        path = newPath;
    }
}
