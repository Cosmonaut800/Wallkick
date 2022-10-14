using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Hurtbox : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == 7 && !other.isTrigger) //Layer 7 == Enemy
		{
			other.gameObject.GetComponent<BaseEnemyController>().Damage(10.0f);
		}
	}
}
