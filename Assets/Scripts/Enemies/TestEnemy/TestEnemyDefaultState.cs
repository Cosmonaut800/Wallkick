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
		rb = controller?.rb;
		controller.hurtbox.SetRadius(3.0f);
	}

	public override State RunCurrentState()
	{
		Vector3 lateral = new Vector3(1.0f, 0.0f, 1.0f);
		controller.transform.rotation = Quaternion.LookRotation(controller.target.transform.position - controller.transform.position);
		rb?.AddForce(controller.speed * Vector3.Scale(Vector3.Normalize(controller.target.transform.position - controller.transform.position), lateral), ForceMode.Acceleration);

		if(Vector3.Distance(controller.transform.position, controller.target.position) < 2.0f && controller.hurtbox.timer < 0.0f)
		{
			controller.hurtbox.Activate(1.0f);
		}
		
		return this;
	}
}
