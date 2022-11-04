using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SubStateGroundedIdle : SubState
{
	private GameObject player;
	private PlayerController controller;
	private Rigidbody rb;
	private Transform graphics;
	private Transform cam;
	private float coefficient = 0.0f;

	public override void Initialize(GameObject parent, State newParentState)
	{
		player = parent;
		parentState = newParentState;
		controller = parent.GetComponent<PlayerController>();
		rb = parent.GetComponent<Rigidbody>();
		graphics = GameObject.Find("Player/Graphics").transform;
		cam = parent.transform.Find("Main Camera");
	}

	public override SubState RunCurrentSubState()
	{
		Vector3 slope = AverageFloors(controller.floors);
		Vector3 forceDir = Quaternion.Euler(0.0f, cam.rotation.eulerAngles.y, 0.0f) * controller.movementVector;
		Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, slope);

		//Coefficient relating various parameters to the slope of the floor currently being stood on.
		//coefficient = Mathf.Max(0.0f, 1.0f - 18800.0f * Mathf.Abs(Mathf.Pow((Mathf.Abs(Vector3.Dot(Vector3.down, AverageFloors(controller.floors))) - 1.0f), 8.0f)));
		if (Vector3.Dot(slope, Vector3.up) > 0.707f)
		{
			coefficient = 1.0f;
			rb.AddForce(controller.speed * (slopeRotation * forceDir));
		}
		else
		{
			coefficient = 0.0f;
			rb.AddForce(0.1f * controller.speed * (slopeRotation * forceDir));
		}

		rb.drag = coefficient * 8.0f;

		controller.animator.SetFloat(
			"Grounded.Idle-Run",
			Mathf.Min(1.0f, rb.velocity.magnitude * 0.1f) //This is the wrong way to handle this but I'll figure it out later
		);

		float turnSmoothVelocity = 0.0f;
		if (controller.movementVector.magnitude > 0.1f)
		{
			float targetAngle = Mathf.Atan2(controller.movementVector.x, controller.movementVector.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			controller.angle = Mathf.SmoothDampAngle(graphics.rotation.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.04f);
			graphics.rotation = Quaternion.Euler(0.0f, controller.angle, 0.0f);
		}

		if (CheckJumpConditions())
		{
			State nextState = new PlayerStateAerial();

			controller.shortenJump = false;
			controller.jumpCooldown = 0.04f + Utility.TIME_EPSILON;
			controller.jumpAnticipate = 0.0f;
			rb.AddForce(Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * controller.jumpHeight), ForceMode.VelocityChange);
			if (controller.platform != null) rb.AddForce(controller.platform.GetPointVelocity(controller.transform.position), ForceMode.VelocityChange);
			nextState.Initialize(player);
			parentState.nextState = nextState;
		}

		if (controller.attack)
		{
			P_SubStateGroundedAttack1 nextSubState = new P_SubStateGroundedAttack1();
			nextSubState.Initialize(player, parentState);

			Debug.Log("Attack received");
			controller.attack = false;

			return nextSubState;
		}

		return this;
	}

	private Vector3 AverageFloors(List<ContactPoint> floors)
	{
		Vector3 result = Vector3.zero;

		for (int i = 0; i < floors.Count; i++)
		{
			result += floors[i].normal;
		}

		result = Vector3.Normalize(result);

		return result;
	}

	private bool CheckJumpConditions()
	{
		return controller.jumpCooldown < 0.0f && controller.jumpAnticipate > 0.0f && coefficient > 0.5f;
	}
}
