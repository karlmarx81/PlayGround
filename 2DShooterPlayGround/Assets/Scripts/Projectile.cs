using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float attackPower = 1f;
    public float maxDistance = 500f;    
    public float lifeSpan = 2f;
    public float launchForce = 500f;
    public GameObject ParentObj { get { return parentObject; } set { parentObject = value; } }

    Rigidbody2D rb;
    Vector2 initPos;
    float lifeTime = 0f;
    GameObject parentObject;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        initPos = transform.position;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, initPos) > maxDistance)
        {
            Destroy(gameObject);
        }

        lifeTime += Time.deltaTime;

        if (lifeTime >= lifeSpan)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(Vector2 direction, float forceMultiplier)
    {
        rb.AddForce(direction * forceMultiplier * launchForce);                        
    }

    public void Rotate(float angle)
    {
        rb.SetRotation(angle);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Projectile has collided with : " + col.gameObject.name);
        //EnemyController enemy = col.gameObject.GetComponent<EnemyController>();

        //if (enemy != null)
        //{
        //    enemy.Fix();
        //}        

        if (col.gameObject.GetInstanceID() == parentObject.GetInstanceID())
        {
            Debug.Log("Same Side : " + col.gameObject.GetInstanceID() + ", " + parentObject.GetInstanceID());
            return;
        }

        Projectile proj = col.gameObject.GetComponent<Projectile>();
        if (proj != null && proj.parentObject == parentObject)
        {
            return;
        }

        AIController enemyAi = col.gameObject.GetComponent<AIController>();
        if (enemyAi != null)
        {
            enemyAi.ChangeHealth(attackPower);
        }

        Destroy(gameObject);
    }
}
