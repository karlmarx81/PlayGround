using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public GameObject projectile;
    public Transform muzzleTransform;
    public float rateOfFire;
    public float triggerInterval;
    public float accuracy;
    public int shotCount;
    public float spreadAngle;
    public bool holdShootingDebug;

    public bool CanShoot { get { return canShoot; } set { canShoot = value; } }
    public GameObject WeaponOwner { get { return WeaponOwner; } set { weaponOwner = value; } }

    GameObject aimingTarget;
    GameObject weaponOwner;
    float rofTimer;
    bool canShoot = true;
    

    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 aimDirection = GetAimingDirection();
        SetWeaponDirection(aimDirection);

        if (CheckROF() && canShoot == true)
        {
            if (aimingTarget != null)
            {
                StartCoroutine(FireProjectile(aimDirection));
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

        if (canShoot == true)
        {
            rofTimer -= Time.deltaTime;
        }
        
        return false;
    }

    Vector3 GetAimingDirection()
    {
        Vector3 targetVector;
        if (aimingTarget == null)
        {
            targetVector = Vector3.zero;
        }
        else
        {
            targetVector = aimingTarget.transform.position - transform.position;
        }        
        float baseAngle = Mathf.Atan2(targetVector.y, targetVector.x) * Mathf.Rad2Deg;
        Vector3 baseDirVector = new Vector3(0f, 0f, baseAngle);

        return baseDirVector;
    }

    IEnumerator FireProjectile(Vector3 direction)
    {
        if (holdShootingDebug)
        {
            yield break;
        }        

        float spreadAngleStep = spreadAngle / shotCount;
        float curAngleStep = direction.z - (spreadAngle / 2f);

        for (int i = 0; i < shotCount; i++)
        {
            GameObject projInstance = Instantiate(projectile, muzzleTransform.position, muzzleTransform.rotation);
            Projectile projComp = projInstance.gameObject.GetComponent<Projectile>();
            float randomAngleDiff = Random.Range(-accuracy, accuracy);

            if (projComp != null)
            {
                projComp.ParentObj = weaponOwner;
                float angletoSet = curAngleStep + randomAngleDiff;
                projComp.Launch(Helper.DegreeToVector2(angletoSet), 1f);
                projComp.Rotate(angletoSet);
            }
            curAngleStep += spreadAngleStep;
            yield return new WaitForSeconds(triggerInterval);

            //float randomAngleDiff = Random.RandomRange(-accuracy, accuracy);
            //projComp.Launch(DegreeToVector2(direction.z + randomAngleDiff), 300f);        
        }
    }

    void SetWeaponDirection(Vector3 directionVector)
    {
        transform.eulerAngles = directionVector;
    }

    public void SetTarget(GameObject target)
    {
        aimingTarget = target;
    }    
}
