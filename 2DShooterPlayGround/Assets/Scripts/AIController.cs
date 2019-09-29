using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIController : MonoBehaviour
{
    public GameObject target;
    public float moveSpd = 10f;
    public float nextWaypointDist = 3f;
    public Animator anim;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker mySeeker;
    Rigidbody2D myRb;    

    void Start()
    {
        mySeeker = GetComponent<Seeker>();
        myRb = GetComponent<Rigidbody2D>();        

        InvokeRepeating("UpdatePath", 0f, 0.5f);        
    }

    void UpdatePath()
    {
        if (mySeeker.IsDone())
        {
            mySeeker.StartPath(myRb.position, target.transform.position, OnPathComplete);
        }
    }

    
    void Update()
    {
        if (path == null)
        {
            //Debug.Log("Update Routine Escaped - Path is null : AIController");
            //return;
        }

        if (currentWaypoint > path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            //Debug.Log("Update Routine Escaped - reached End of Path : AIController");
            //return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - myRb.position).normalized;
        Vector2 force = direction * moveSpd * Time.deltaTime;

        myRb.AddForce(force);

        float distance = Vector2.Distance(myRb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDist)
        {
            currentWaypoint++;
        }

        if (anim != null)
        {
            anim.SetFloat("MoveX", myRb.velocity.x);
            anim.SetFloat("MoveY", myRb.velocity.y);
            Debug.Log("MoveX value is : " + myRb.velocity.x + " ,MoveY value is : " + myRb.velocity.y);
        }        
        
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            currentWaypoint = 0;
        }
    }
}
