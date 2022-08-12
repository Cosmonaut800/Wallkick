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

	private Vector3 forceDir;

	private float fallTimer = 0.1f;

	public override void Initialize(GameObject parent)
	{
		player = parent;
		rb = player.GetComponent<Rigidbody>();
		controller = player.GetComponent<PlayerController>();
		graphics = GameObject.Find("Player/Graphics").transform;
		cam = player.transform.Find("Main Camera");
		rb.drag = 8.0f;
		controller.touchedGround = true;
		controller.animator.SetInteger("PlayerState", 1);
	}

	public override State RunCurrentState()
	{
		float coefficient;
		Vector3 slope = AverageFloors(controller.floors);
		Quaternion slopeRotation;
		float slopeRatio;

		//Coefficient relating various parameters to the slope of the floor currently being stood on.
		coefficient = Mathf.Max(0.0f, 1.0f - 18800.0f * Mathf.Abs(Mathf.Pow((Mathf.Abs(Vector3.Dot(Vector3.down, AverageFloors(controller.floors))) - 1.0f), 8.0f)));
		rb.drag = coefficient * 8.0f;

		forceDir = Quaternion.Euler(0.0f, cam.rotation.eulerAngles.y, 0.0f) * controller.movementVector;
		slopeRotation = Quaternion.FromToRotation(Vector3.up, slope);

		slopeRatio = Vector3.Dot(forceDir, slope);
		if (slopeRatio > 0.01f) coefficient = 1.0f;
		controller.debugString = fallTimer.ToString();
		rb.AddForce(coefficient * controller.speed * (slopeRotation * forceDir));

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
			//rb.drag = 8.0f;
		}

		if (rb.velocity.magnitude > 0.2f)
		{
			ApplyFriction(slope);
		}
		else
		{
			rb.AddForce(coefficient * -rb.velocity, ForceMode.VelocityChange);
			rb.AddForce(coefficient * -Physics.gravity, ForceMode.Acceleration);
			//Mathf.Lerp(1.0f, 0.0f, 1 - Mathf.Pow(Mathf.Abs(Vector3.Dot(AverageFloors(controller.floors), Vector3.Normalize(Physics.gravity))), 0.333f))
		}

		ClampGround();

		if (CheckJumpConditions())
		{
			Debug.Log("Left grounded state because of jump.");
			PlayerStateAerial nextState = new PlayerStateAerial();
			controller.animator.SetInteger("PlayerState", 2);

			controller.shortenJump = false;
			controller.jumpCooldown = 0.04f + Utility.TIME_EPSILON;
			controller.jumpAnticipate = 0.0f;
			rb.AddForce(Vector3.up * coefficient * Mathf.Sqrt(-2 * Physics.gravity.y * controller.jumpHeight), ForceMode.VelocityChange);
			nextState.Initialize(player);
			return nextState;
		}
		if (CheckFallConditions())
		{
			Debug.Log("Left grounded state because of fall.");
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

	private bool ClampGround()
	{
		bool result;
		Vector3 displacement;

		//result = rb.SweepTest(Vector3.down, out controller.hit, 1.0f);
		result = Physics.SphereCast(controller.transform.position, 0.45f, Vector3.down, out controller.hit, 1.25f);
		displacement = (controller.hit.distance - 0.55f) * Vector3.down;

		if (result)
		{
			rb.MovePosition(controller.transform.position + displacement);
		}

		return result;
	}

	//----State Transitions----------------
	private bool CheckJumpConditions()
	{
		return controller.jumpCooldown < 0.0f && controller.jumpAnticipate > 0.0f;
	}

	private bool CheckFallConditions()
	{
		if (controller.floors.Count <= 0) fallTimer -= Time.fixedDeltaTime;
		else fallTimer = 0.1f;
		return (controller.floors.Count <= 0 && fallTimer < 0.0f);
	}
}
