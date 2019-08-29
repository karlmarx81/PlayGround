using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public GameObject projectile;
    public Transform muzzleTransform;
    public float rateOfFire;
    public float accuracy;
    public int shotCount;
    public float spreadAngle;


    GameObject aimingTarget;
    float rofTimer;
    

    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 aimDirection = GetAimingDirection();
        SetWeaponDirection(aimDirection);

        if (CheckROF())
        {            
            GameObject projInstance = Instantiate(projectile, muzzleTransform.position, muzzleTransform.rotation);           

            Projectile projComp = projInstance.gameObject.GetComponent<Projectile>();
            if (projComp != null)
            {
                projComp.Launch(DegreeToVector2(aimDirection.z), 300f);                
            }            
        }
    }

    bool CheckROF()
    {
        if (rofTimer <= 0)
        {
            rofTimer = rateOfFire;
            return true;
        }        

        rofTimer -= Time.deltaTime;
        return false;
    }

    Vector3 GetAimingDirection()
    {
        Vector3 targetVector = aimingTarget.transform.position - transform.position;
        float baseAngle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
        Vector3 baseDirVector = new Vector3(0f, 0f, baseAngle);

        return baseDirVector;
    }

    void SetWeaponDirection(Vector3 directionVector)
    {
        transform.eulerAngles = directionVector;
    }

    public void SetTarget(GameObject target)
    {
        aimingTarget = target;
    }

    public Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
}
