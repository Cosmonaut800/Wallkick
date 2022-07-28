using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateGrounded : State
{
	private GameObject player;
	private Rigidbody rb;
	private ThirdPersonPlayer controller; //Will be replaced with "PlayerController" class
	private Transform graphics;
	private Transform cam;
	private bool jump;

	private Vector3 forceDir;

	public override void Initialize(GameObject parent)
	{
		player = parent;
		rb = player.GetComponent<Rigidbody>();
		controller = player.GetComponent<ThirdPersonPlayer>();
		graphics = GameObject.Find("Player/Graphics").transform;
		cam = player.transform.Find("Main Camera");
		rb.drag = 8.0f;
		controller.touchedGround = true;
		controller.animator.SetInteger("PlayerState", 1);
	}

	public override State RunCurrentState()
	{
		float coefficient = Mathf.Max(0.0f, 1.0f - 20.0f * Mathf.Abs(Mathf.Pow((Mathf.Abs(Vector3.Dot(Vector3.Normalize(Physics.gravity), AverageFloors(controller.floors))) - 1.0f), 4.0f)));
		rb.drag = coefficient * 8.0f;

		forceDir = Quaternion.Euler(0.0f, cam.rotation.eulerAngles.y, 0.0f) * controller.movementVector;
		rb.AddForce(controller.speed * forceDir);

		controller.animator.SetFloat(
			"Grounded.Idle-Run", 
			Mathf.Min(1.0f, rb.velocity.magnitude * 0.1f)
		);

		float turnSmoothVelocity = 0.0f;
		if (controller.movementVector.magnitude > 0.1f)
		{
			float targetAngle = Mathf.Atan2(controller.movementVector.x, controller.movementVector.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(graphics.rotation.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.04f);
			graphics.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
			rb.drag = 8.0f;
		}

		if (rb.velocity.magnitude > 0.2f)
		{
			ApplyFriction(AverageFloors(controller.floors));
		}
		else
		{
			rb.AddForce(coefficient * -rb.velocity, ForceMode.VelocityChange);
			rb.AddForce(coefficient * -Physics.gravity, ForceMode.Acceleration);
			//Mathf.Lerp(1.0f, 0.0f, 1 - Mathf.Pow(Mathf.Abs(Vector3.Dot(AverageFloors(controller.floors), Vector3.Normalize(Physics.gravity))), 0.333f))
		}

		if (CheckJumpConditions())
		{
			PlayerStateAerial nextState = new PlayerStateAerial();
			controller.animator.SetInteger("PlayerState", 2);

			controller.shortenJump = false;
			controller.jumpCooldown = 0.04f + Utility.TIME_EPSILON;
			controller.jumpAnticipate = 0.0f;
			rb.AddForce(Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * controller.jumpHeight), ForceMode.VelocityChange);
			nextState.Initialize(player);
			return nextState;
		}
		if (CheckFallConditions())
		{
			PlayerStateAerial nextState = new PlayerStateAerial();
			controller.animator.SetInteger("PlayerState", 2);

			controller.touchedGround = false;
			controller.shortenJump = true;
			nextState.Initialize(player);
			return nextState;
		}
		else
		{
			return this;
		}
	}

	private Vector3 AverageFloors(List<ContactPoint> floors)
	{
		Vector3 result = Vector3.zero;

		for (int i = 0; i < floors.Count; i++)
		{
			result = result + floors[i].normal;
		}

		result = Vector3.Normalize(result);

		return result;
	}

	private void ApplyFriction(Vector3 normal)
	{
		rb.AddForce(controller.maxFriction * Mathf.Abs(Vector3.Dot(Physics.gravity, normal)) * -Vector3.Normalize(rb.velocity), ForceMode.Acceleration);
		//          Friction coefficient   *           Force against surface normal     in the direction opposite of motion
	}

	//----State Transitions----------------
	private bool CheckJumpConditions()
	{
		return controller.jumpCooldown < 0.0f && controller.jumpAnticipate > 0.0f;
	}

	private bool CheckFallConditions()
	{
		return controller.floors.Count <= 0;
	}
}
