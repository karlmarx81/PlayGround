using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float maxDistance = 500f;
    public GameObject parentObj;

    Rigidbody2D rb;
    Vector2 initPos;    

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
    }

    public void Launch(Vector2 direction, float force)
    {
        rb.AddForce(direction * force);        
    }    

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Projectile has collided with : " + col.gameObject.name);
        EnemyController enemy = col.gameObject.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.Fix();
        }

        Destroy(gameObject);
    }
}
