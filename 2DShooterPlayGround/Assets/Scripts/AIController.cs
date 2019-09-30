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
    public GameObject weaponPrefab;
    public float shootingRange = 10f;
    public float chaseRange = 7f;
    public float aiDecisionTime = 0.5f;
    public float maxHealth = 5;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool canShoot = false;
    bool canMove = true;
    Seeker mySeeker;
    Rigidbody2D myRb;
    WeaponBase weapon;
    float currentHealth;

    float targetDist;
    float decisionTimeToCheck = 0f;

    void Start()
    {
        mySeeker = GetComponent<Seeker>();
        myRb = GetComponent<Rigidbody2D>();
        weapon = weaponPrefab.gameObject.GetComponent<WeaponBase>();

        currentHealth = maxHealth;

        if (weapon != null)
        {
            weapon.WeaponOwner = transform.gameObject;
        }

        decisionTimeToCheck = aiDecisionTime;

        if (weapon != null)
        {
            weapon.SetTarget(target);
            //place this to update function.
        }        
    }

    void UpdatePath(Vector3 targetPos)
    {
        if (mySeeker.IsDone())
        {
            mySeeker.StartPath(myRb.position, targetPos, OnPathComplete);
        }        
    }

    void Update()
    {
        CheckHealth();
        ShootingCheck();        

        targetDist = Vector2.Distance(myRb.position, target.transform.position);

        if (decisionTimeToCheck <= 0)
        {
            //Run AI decision
            if (targetDist < chaseRange)
            {
                HoldPosition();
                Debug.Log("Hold Position : " + targetDist);
            }
            else
            {
                Chase();
                Debug.Log("Chasing : " + targetDist);
            }

            decisionTimeToCheck = aiDecisionTime;
        }
        else
        {
            decisionTimeToCheck -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        if (path == null)
        {
            Debug.Log("Update Routine Escaped - Path is null : AIController");
            return;
        }
        else
        {
            Debug.Log("Path id : " + path.pathID);
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            Debug.Log("Update Routine Escaped - reached End of Path : AIController");
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - myRb.position).normalized;
        Vector2 force = direction * moveSpd * Time.deltaTime;
        //Debug.Log(force.magnitude);

        if (canMove)
        {
            myRb.AddForce(force);
        }                

        float distance = Vector2.Distance(myRb.position, path.vectorPath[currentWaypoint]);

        if (anim != null)
        {
            anim.SetFloat("MoveX", myRb.velocity.x);
            anim.SetFloat("MoveY", myRb.velocity.y);
            Debug.Log("MoveX value is : " + myRb.velocity.x + " ,MoveY value is : " + myRb.velocity.y);
        }

        if (distance < nextWaypointDist)
        {
            currentWaypoint++;
        }        
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            Debug.Log("Path Calculated");
        }
    }

    void ShootingCheck()
    {
        if (targetDist <= shootingRange)   //target is in shooting range
        {
            canShoot = true;
            weapon.CanShoot = canShoot;
        }
        else
        {
            canShoot = false;
            weapon.CanShoot = canShoot;
        }
    }

    void Idle()
    {
        //Enemy will stand still when there is no target.
    }

    void Chase()
    {
        //Track target within shooting range.
        //Based on target's position.         
        if (canMove == false)        
            canMove = true;

        UpdatePath(target.transform.position);
    }

    void HoldPosition()
    {
        //Enough distance to keep firing. stop moving and stand still.
        //When distance increases beyond the threshold. end this routine.
        if (canMove == true)
            canMove = false;
    }

    void AttackTarget()
    {
        //allow AI's weapon to fire.
    }

    public void ChangeHealth(float damage)
    {
        currentHealth -= damage;
    }

    void CheckHealth()
    {
        if (currentHealth <= 0f)
        {
            //this one is dead
            Destroy(gameObject);
        }
    }
}
