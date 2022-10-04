using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyController : BaseEnemyController
{
	public Transform target;

	void SpawnHurtbox(Vector3 position, float radius)
	{
		SphereCollider hurtbox = new SphereCollider();
		hurtbox.center = position;
		hurtbox.radius = radius;

		hurtboxes.Add(hurtbox);
	}
}
