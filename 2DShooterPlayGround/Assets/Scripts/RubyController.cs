using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    [Header("General Settings")]
    public float moveSpeed = 0.1f;    
    public int maxHelath = 5;
    public int health { get { return currentHelath; } }
    public float invincibleTime = 2.0f;

    [Space]
    [Header("Weapon Settings")]
    public GameObject weaponPrefab;
    public GameObject projectile;
    public float projForce;
    
    Rigidbody2D rb;
    int currentHelath;
    bool isInvincible;
    float invincibleTimer;
    Vector2 lookDirection = new Vector2(1,0);
    Vector2 moveVector;
    Animator anim;
    WeaponBase weapon;
    GameObject enemyTarget;
    bool canShoot;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHelath = maxHelath;
        anim = GetComponent<Animator>();
        weapon = weaponPrefab.gameObject.GetComponent<WeaponBase>();
        if (weapon != null)
        {
            weapon.WeaponOwner = transform.gameObject;
        }
    }

    void Update()
    {
        float horiX = Input.GetAxis("Horizontal");
        float vertY = Input.GetAxis("Vertical");
        moveVector = new Vector2(horiX, vertY);
        if (moveVector.magnitude > 1.0f)
        {
            moveVector.Normalize();
        }       

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

        if (Input.GetButton("Jump"))
        {
            Launch();
        }

        FindTarget();
        ShootingCheck();
    }

    void ShootingCheck()
    {
        if (Mathf.Approximately(moveVector.magnitude, 0.0f))
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
        UIHealthBar.instance.SetValue(currentHelath / (float)maxHelath);
        //Debug.Log(currentHelath + " / " + maxHelath);
    }

    void Launch()
    {
        GameObject fireObj = Instantiate(projectile, rb.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile proj = fireObj.gameObject.GetComponent<Projectile>();
        proj.Launch(lookDirection, projForce);
        anim.SetTrigger("Launch");
    }

    void FindTarget()
    {        
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = Mathf.Infinity;

        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 diff = enemies[i].transform.position - transform.position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                enemyTarget = enemies[i].gameObject;
                distance = curDistance;
            }
        }
        //Debug.Log("Target is : " + enemyTarget);

        if (weapon != null)
        {
            if (enemyTarget != null)
            {
                weapon.SetTarget(enemyTarget);
            }            
        }
    }
}
