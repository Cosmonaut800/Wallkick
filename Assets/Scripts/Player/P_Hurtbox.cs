using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Hurtbox : Hurtbox
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 7 && !other.isTrigger) //Layer 7 == Enemy
		{
			Debug.Log(other.name);
			other.gameObject.GetComponent<BaseEnemyController>()?.Damage(10.0f);
		}
	}
}
