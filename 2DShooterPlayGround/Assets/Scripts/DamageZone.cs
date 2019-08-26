using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerStay2D(Collider2D collision)
	{
		RubyController controller = collision.GetComponent<RubyController>();

		if (controller != null)
		{
			controller.ChangeHealth(-1);
		}
	}
}
