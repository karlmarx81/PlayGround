using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 100f;
    public int maxHealth;
    public bool isVerticalMove;
    public float changeDirPeriod = 3.0f;

    Rigidbody2D rb;
    Animator anim;
    int currentHealth;
    float timer;
    float reverseFactor = 1f;
    bool broken = true;
        
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timer = changeDirPeriod;
    }

    void Update()
    {
        if (!broken)
            return;

        Vector2 position = rb.position;

        if (isVerticalMove)
        {
            position.y += reverseFactor * moveSpeed * Time.deltaTime;
            anim.SetFloat("MoveX", 0);
            anim.SetFloat("MoveY", reverseFactor);
        }
        else
        {
            position.x += reverseFactor * moveSpeed * Time.deltaTime;
            anim.SetFloat("MoveY", 0);
            anim.SetFloat("MoveX", reverseFactor);
        }

        rb.MovePosition(position);

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            reverseFactor *= -1;
            timer = changeDirPeriod;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController controller = collision.gameObject.GetComponent<RubyController>();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        broken = false;
        rb.simulated = false;
        anim.SetTrigger("Fixed");
    }
}
