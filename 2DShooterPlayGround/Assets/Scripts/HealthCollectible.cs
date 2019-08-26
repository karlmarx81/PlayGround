using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Obj that entered the trigger : " + collision.name);

        RubyController rubyController = collision.GetComponent<RubyController>();
        if (rubyController != null)
        {
            if (rubyController.health < rubyController.maxHelath)
            {
                rubyController.ChangeHealth(1);
                Destroy(gameObject);
            }
            
        }
    }
}
