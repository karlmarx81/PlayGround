using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float moveSpeed = 0.1f;    
    public int maxHelath = 5;
    public int health { get { return currentHelath; } }
    public float invincibleTime = 2.0f;
    
    Rigidbody2D rb;
    int currentHelath;
    bool isInvincible;
    float invincibleTimer;
    Vector2 lookDirection = new Vector2(1,0);
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHelath = maxHelath;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float horiX = Input.GetAxis("Horizontal");
        float vertY = Input.GetAxis("Vertical");
        Vector2 moveVector = new Vector2(horiX, vertY);

        if (!Mathf.Approximately(moveVector.x, 0.0f) || !Mathf.Approximately(moveVector.y, 0.0f))
        {
            lookDirection.Set(moveVector.x, moveVector.y);
            lookDirection.Normalize();
        }       

        anim.SetFloat("LookX", lookDirection.x);
        anim.SetFloat("LookY", lookDirection.y);
        anim.SetFloat("Speed", moveVector.magnitude);

        Vector2 position = rb.position;
        position = position + moveVector * moveSpeed * Time.deltaTime;
        rb.MovePosition(position);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = invincibleTime;
            anim.SetTrigger("Hit");
        }

        currentHelath = Mathf.Clamp(currentHelath + amount, 0, maxHelath);       

        //Debug.Log(currentHelath + " / " + maxHelath);
    }
}
