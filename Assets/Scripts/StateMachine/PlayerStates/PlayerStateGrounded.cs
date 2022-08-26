using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateGrounded : State
{
	private GameObject player;
	private Rigidbody rb;
	private PlayerController controller; //Will be replaced with "PlayerController" class
	private Transform graphics;
	private Transform cam;
	private bool jump;

	private float coefficient;

	private Vector3 forceDir;

	private float coyoteTime = 0.1f;

	public override void Initialize(GameObject parent)
	{
		player = parent;
		rb = player.GetComponent<Rigidbody>();
		controller = player.GetComponent<PlayerController>();
		graphics = GameObject.Find("Player/Graphics").transform;
		cam = player.transform.Find("Main Camera");
		rb.drag = 8.0f;
		controller.touchedGround = true;
		controller.animator.SetTrigger("PlayerState.Grounded");  // Animator
	}

	public override State RunCurrentState()
	{
		Vector3 slope = AverageFloors(controller.floors);
		Quaternion slopeRotation;
		float slopeRatio;
		
		// Animator
		controller.animator.SetFloat(
			"Grounded.Idle-Run", 
			Mathf.Min(1.0f, rb.velocity.magnitude * 0.1f)
		);

		ClampGround();

		if (controller.platform != null)
		{
			controller.groundVelocity = controller.platform.GetPointVelocity(controller.transform.position + Vector3.down);
			rb.MovePosition(controller.transform.position + controller.platform.GetPointVelocity(controller.transform.position + Vector3.down) * Time.fixedDeltaTime);
			graphics.rotation = Quaternion.Euler(0.0f, graphics.rotation.eulerAngles.y + controller.platform.angularVelocity.y, 0.0f);
		}
		else
		{
			controller.groundVelocity = Vector3.zero;
		}

		forceDir = Quaternion.Euler(0.0f, cam.rotation.eulerAngles.y, 0.0f) * controller.movementVector;
		slopeRotation = Quaternion.FromToRotation(Vector3.up, slope);

		slopeRatio = Vector3.Dot(forceDir, slope);
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

<<<<<<< HEAD
=======
		controller.animator.SetFloat(
			"Grounded.Idle-Run", 
			Mathf.Min(1.0f, rb.velocity.magnitude * 0.1f) //This is the wrong way to handle this but I'll figure it out later
		);

>>>>>>> main
		float turnSmoothVelocity = 0.0f;
		if (controller.movementVector.magnitude > 0.1f)
		{
			float targetAngle = Mathf.Atan2(controller.movementVector.x, controller.movementVector.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			controller.angle = Mathf.SmoothDampAngle(graphics.rotation.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.04f);
			graphics.rotation = Quaternion.Euler(0.0f, controller.angle, 0.0f);
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

		if (CheckJumpConditions())
		{
			PlayerStateAerial nextState = new PlayerStateAerial();

			controller.shortenJump = false;
			controller.jumpCooldown = 0.04f + Utility.TIME_EPSILON;
			controller.jumpAnticipate = 0.0f;
			rb.AddForce(Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * controller.jumpHeight), ForceMode.VelocityChange);
			if (controller.platform != null) rb.AddForce(controller.platform.GetPointVelocity(controller.transform.position), ForceMode.VelocityChange);
			nextState.Initialize(player);
			return nextState;
		}
		if (CheckFallConditions())
		{
			PlayerStateAerial nextState = new PlayerStateAerial();

			if (controller.platform != null) rb.AddForce(controller.platform.GetPointVelocity(controller.transform.position), ForceMode.VelocityChange);
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
