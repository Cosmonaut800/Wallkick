using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Hurtbox : Hurtbox
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 6 && !other.isTrigger) //Layer 6 == Player
		{
			other.gameObject.GetComponent<PlayerController>().Damage(10.0f);
		}
	}
}
