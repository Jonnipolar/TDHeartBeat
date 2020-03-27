using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Virus))]
public class VirusMovement : MonoBehaviour
{
    public Stack<Vector2> path;
    public LayerMask blockingLayer;  
    public bool stuck;
    public Vector2 currentPathPoint;
    private BoxCollider2D boxCollider;  
    float inverseMovementTime;
    Rigidbody2D rb;
    Animator animator;
    bool moving;
    private Virus virus;
    private bool movementChange = false;
    private List<Color> colors;
    private int colorIndex;
    private Vector2 goal;
    LineRenderer linerenderer;

    private void Awake()
    {
        colors = new List<Color> { Color.white, Color.black, Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta, Color.red, Color.yellow };
        if (linerenderer == null)
        {
            linerenderer = GetComponent<LineRenderer>();
        }
        linerenderer.material = new Material(Shader.Find("Sprites/Default"));
        linerenderer.widthMultiplier = 0.1f;
        linerenderer.sortingOrder = 2;
        colorIndex = 0;
    }

    void Start()
    {
        stuck = false;
        boxCollider = GetComponent <BoxCollider2D>();
        animator = GetComponent<Animator>();
        virus = GetComponent<Virus>();
        rb = GetComponent<Rigidbody2D>();
        /*Debug.Log(virus.currentMovementTime);*/
        inverseMovementTime = 1.0f / virus.currentMovementTime;
        
        currentPathPoint = rb.position;

    }

    void followPath()
    {
        if(!moving)
        {
            Vector2 next = path.Pop();
            linerenderer.positionCount = path.Count + 1;
            MoveTo(next);
        }
    }

    void MoveTo(Vector3 pos)
    {
        StartCoroutine(MovementCoroutine(pos));
    }

    Stack<Vector2> FindSideSteps(Stack<Vector2> paths, Vector3 uVector, Vector3 startPos, Vector3 endPos, int r)
    {
        Vector3 sideStep;
        for(float i = 0.2f; i < 5.0f; i += 0.2f) {
            sideStep = uVector * i;
            for(int k =  15; k < 360; k += 15)
            {
                sideStep = Quaternion.AngleAxis(k, Vector3.forward) * sideStep;
                Vector3 sideStepPosition = startPos + sideStep;
                if(!ObstacleCheck(startPos, sideStepPosition) && !ObstacleCheck(sideStepPosition, endPos))
                {
                    paths.Push(sideStepPosition);
                    return paths;
                }
                else if (r < 3) 
                {
                    r++;
                    paths = FindSideSteps(paths, uVector, sideStepPosition, endPos, r);
                    if(paths.Count > 0)
                    {
                        paths.Push(sideStepPosition);
                        return paths;
                    }
                }
            }
        }
        return paths;
    }

    IEnumerator MovementCoroutine(Vector3 endPos)
    {
        moving = true;

        Vector3 movementVector = endPos - (Vector3)rb.position;
        Vector3 uVector = movementVector.normalized;
        float sqrDist;

        if(ObstacleCheck(rb.position, endPos)) {
            bool blocked = true;
            Stack<Vector2> steps = FindSideSteps(new Stack<Vector2>(), uVector, rb.position, endPos, 0);
            if(steps.Count > 0) 
            {
                while(steps.Count > 0)
                {
                    Vector2 step = steps.Pop();
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
                }
                Debug.Log("SIDE AND CONTINUE");
            }
            else 
            {
                steps = FindSideSteps(new Stack<Vector2>(), uVector, rb.position, path.Peek(), 0);
                if(steps.Count > 0) 
                {
                    while(steps.Count > 0) 
                    {
                        Vector2 step = steps.Pop();
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
            else 
            {
                Debug.Log("BLOCKED!");
                moving = false;
                path = null;
                stuck = true;
                yield break;
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
        currentPathPoint = endPos;
        moving = false;
    }

    bool ObstacleCheck(Vector3 currPos, Vector3 endPos)
    {
        // boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(currPos, endPos, blockingLayer);
        // boxCollider.enabled = true;
        
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

        if(path != null )
        {
            if(path.Count > 0)
            {
                linerenderer.enabled = true;
                linerenderer.SetPosition(0, transform.position);
                int i = 1;
                foreach (var coord in path)
                {
                    linerenderer.SetPosition(i, coord);
                    i++;
                }
                followPath();    
            }
            else if(path.Count == 0 && checkInGoalZone())
            {
                Destroy(gameObject);
            }
        }
        else
        {
            linerenderer.enabled = false;
            Animate(new Vector3(0, 0, 0));
        }

    }

    public void SetPath(Stack<Vector2> newPath, Vector2 goal) 
    {
        this.goal = goal;
        path = newPath;
        if(linerenderer == null)
        {
            linerenderer = GetComponent<LineRenderer>();
        }

        if(colorIndex >= colors.Count - 1) { colorIndex = 0;  }
        else { colorIndex++; }

        linerenderer.positionCount = path.Count + 1;
        linerenderer.startColor = colors[colorIndex];
        linerenderer.endColor = colors[colorIndex];
    }

    private bool checkInGoalZone()
    {
        if (transform.position.x > goal.x -0.5f && transform.position.x < goal.x + 0.5f &&
            transform.position.y > goal.y -0.5f && transform.position.y < goal.y + 0.5f)
        {
            return true;
        }

        return false;
    }

}
