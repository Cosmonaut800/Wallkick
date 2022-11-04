using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateGrounded : State
{
	private GameObject player;
	private Rigidbody rb;
	private PlayerController controller;
	private Transform graphics;

	private float coefficient;

	private float coyoteTime = 0.1f;

	private SubState currentSubState;

	public override void Initialize(GameObject parent)
	{
		player = parent;
		nextState = this;
		rb = player.GetComponent<Rigidbody>();
		controller = player.GetComponent<PlayerController>();
		graphics = GameObject.Find("Player/Graphics").transform;
		rb.drag = 8.0f;
		controller.touchedGround = true;
		controller.animator.SetTrigger("PlayerState.Grounded");  // Animator
		currentSubState = new P_SubStateGroundedIdle();
		currentSubState.Initialize(parent, this);
	}

	public override State RunCurrentState()
	{
		Vector3 slope = AverageFloors(controller.floors);
		
		// Animator
		controller.animator.SetFloat(
			"Grounded.Idle-Run", 
			Mathf.Min(1.0f, rb.velocity.magnitude * 0.1f)
		);

		ClampGround();

		if (controller.platform != null)
		{
			rb.MovePosition(controller.transform.position + controller.platform.GetPointVelocity(controller.transform.position + Vector3.down) * Time.fixedDeltaTime);
			graphics.rotation = Quaternion.Euler(0.0f, graphics.rotation.eulerAngles.y + controller.platform.angularVelocity.y, 0.0f);
		}

		if (Vector3.Dot(slope, Vector3.up) > 0.707f)
		{
			coefficient = 1.0f;
		}
		else
		{
			coefficient = 0.0f;
		}
		if (rb.velocity.magnitude > 0.2f)
		{
			ApplyFriction(slope);
		}
		else
		{
			rb.AddForce(coefficient * -rb.velocity, ForceMode.VelocityChange);
			rb.AddForce(coefficient * -Physics.gravity, ForceMode.Acceleration);
		}

		if (CheckFallConditions())
		{
			nextState = new PlayerStateAerial();

			if (controller.platform != null) rb.AddForce(controller.platform.GetPointVelocity(controller.transform.position), ForceMode.VelocityChange);
			controller.touchedGround = false;
			controller.shortenJump = true;
			nextState.Initialize(player);
		}
		else
		{
			nextState = this;
		}

		RunSubState();

		return nextState;
	}

	private void RunSubState()
	{
		currentSubState = currentSubState.RunCurrentSubState();
		Debug.Log(currentSubState.ToString());
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

	private void ApplyFriction(Vector3 normal)
	{
		rb.AddForce(controller.maxFriction * Mathf.Abs(Vector3.Dot(Physics.gravity, normal)) * -Vector3.Normalize(rb.velocity), ForceMode.Acceleration);
		//          Friction coefficient   *           Force against surface normal     in the direction opposite of motion
	}

	private bool ClampGround()
	{
		bool result;
		Vector3 displacement;

		if (coyoteTime < 0.1f)
		{
			return false;
		}

		//result = rb.SweepTest(Vector3.down, out controller.hit, 1.0f);
		result = Physics.SphereCast(controller.transform.position + (0.49f * Vector3.down), 0.50f, Vector3.down, out controller.hit, 0.20f);
		controller.platform = controller.hit.rigidbody;
		displacement = (controller.hit.distance - 0.01f) * Vector3.down;

		if (result)
		{
			rb.MovePosition(controller.transform.position + displacement);
		}

		return result;
	}

	//----State Transitions----------------
	private bool CheckJumpConditions()
	{
		return controller.jumpCooldown < 0.0f && controller.jumpAnticipate > 0.0f && coefficient > 0.5f;
	}

	private bool CheckFallConditions()
	{
		if (controller.floors.Count > 0)
		{
			coyoteTime = 0.1f;
			return false;
		}
		else
		{
			coyoteTime -= Time.fixedDeltaTime;
			return coyoteTime < 0.0f;
		}
	}
}
