using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyDefaultState : State
{
	private TestEnemyController controller;
	private Rigidbody rb;

	public override void Initialize(GameObject parent)
	{
		controller = parent.GetComponent<TestEnemyController>();
		rb = controller.rb;
	}

	public override State RunCurrentState()
	{
		controller.transform.rotation = Quaternion.LookRotation(controller.target.transform.position - controller.transform.position);
		rb.AddForce((controller.speed * Vector3.Normalize(controller.target.transform.position - controller.transform.position)) * Time.fixedDeltaTime, ForceMode.VelocityChange);
		
		return this;
	}
}
