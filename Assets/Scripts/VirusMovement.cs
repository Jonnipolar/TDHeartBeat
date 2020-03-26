using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Virus))]
public class VirusMovement : MonoBehaviour
{
    public Stack<Vector2> path;
    public LayerMask blockingLayer;  
    private BoxCollider2D boxCollider;  
    float inverseMovementTime;
    Rigidbody2D rb;
    Animator animator;
    bool moving;
    private Virus virus;
    private bool movementChange = false;
    
    void Start()
    {
        boxCollider = GetComponent <BoxCollider2D>();
        animator = GetComponent<Animator>();
        virus = GetComponent<Virus>();
        rb = GetComponent<Rigidbody2D>();
        Debug.Log(virus.currentMovementTime);
        inverseMovementTime = 1.0f / virus.currentMovementTime;
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

    Vector2 FindSideStep(Vector3 uVector, Vector3 startPos, Vector3 endPos)
    {
        Vector3 sideStep;
        for(float i = 0.2f; i < 3f; i += 0.2f) {
            sideStep = uVector * i;
            for(int k =  20; k < 360; k += 20)
            {
                sideStep = Quaternion.AngleAxis(k, Vector3.forward) * sideStep;
                Vector3 sideStepPosition = startPos + sideStep;
                if(!ObstacleCheck(startPos, sideStepPosition) && !ObstacleCheck(sideStepPosition, endPos))
                {
                    return sideStepPosition;
                }
            }
        }
        return startPos;
    }
    IEnumerator MovementCoroutine(Vector3 endPos)
    {
        moving = true;

        Vector3 movementVector = endPos - (Vector3)rb.position;
        Vector3 uVector = movementVector.normalized;
        float sqrDist;

        if(ObstacleCheck(rb.position, endPos)) {
            bool blocked = true;
            while(blocked)
            {
                Vector2 step = FindSideStep(uVector, rb.position, endPos);
                if(step != rb.position) 
                {
                    Vector3 sideStepVector = step - rb.position;
                    Animate(sideStepVector);
                    sqrDist = sideStepVector.sqrMagnitude;
                
                    while(sqrDist > 0.01f)
                    {
                        Vector2 newPosition = Vector2.MoveTowards(rb.position, step, inverseMovementTime * Time.deltaTime);
                        rb.MovePosition(newPosition);
                        sqrDist = (rb.position - step).sqrMagnitude;
                        yield return null;
                    }
                        Debug.Log("SIDE AND CONTINUE");
                    
                }
                else 
                {
                    step = FindSideStep(uVector, rb.position, path.Peek());
                    if(step != rb.position) 
                    {
                        Vector3 sideStepVector = step - rb.position;
                        Animate(sideStepVector);
                        sqrDist = sideStepVector.sqrMagnitude;
                    
                        while(sqrDist > 0.01f)
                        {
                            Vector2 newPosition = Vector2.MoveTowards(rb.position, step, inverseMovementTime * Time.deltaTime);
                            rb.MovePosition(newPosition);
                            sqrDist = (rb.position - step).sqrMagnitude;
                            yield return null;
                        }
                        Debug.Log("SIDE TOWARdS NEXT AND BREAK");
                        moving = false;
                        yield break;
                    }
                }

                if(!ObstacleCheck(rb.position, endPos))
                {
                    blocked = false;
                    if(ObstacleCheck(rb.position, path.Peek()))
                    {
                        moving = false;
                        Debug.Log("SIDE AND BREAK");
                        yield break;
                    }

                }
            }
        }
        
        sqrDist = movementVector.sqrMagnitude;
        Animate(movementVector);
        while(sqrDist > 0.01f)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, endPos, inverseMovementTime * Time.deltaTime);
            rb.MovePosition(newPosition);
            sqrDist = (transform.position - endPos).sqrMagnitude;
            yield return null;
        }

        moving = false;
    }

    bool ObstacleCheck(Vector3 currPos, Vector3 endPos)
    {
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(currPos, endPos, blockingLayer);
        boxCollider.enabled = true;
        
        // Return true if theres an obstacle
        return hit.transform != null;
    }

    void Animate(Vector3 move)
    {
        animator.SetFloat("Horizontal", move.x);
		animator.SetFloat("Vertical", move.y);
		animator.SetFloat("Magnitude", move.magnitude);
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

        if(path != null && path.Count > 0)
        {
            followPath();    
        }
        else
        {
            Animate(new Vector3(0, 0, 0));
        }
    }

    public void SetPath(Stack<Vector2> newPath) 
    {
        path = newPath;
    }

}
