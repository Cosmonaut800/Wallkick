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

    public override State RunCurrentState()
	{
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
			controller.animator.SetInteger("PlayerState", 1);
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

	public override void Initialize(GameObject parent)
	{
		player = parent;
		rb = player.GetComponent<Rigidbody>();
		controller = player.GetComponent<ThirdPersonPlayer>();
		graphics = GameObject.Find("Player/Graphics").transform;
		cam = player.transform.Find("Main Camera");
		rb.drag = 8.0f;
		controller.touchedGround = true;
	}

	private bool CheckJumpConditions()
	{
		return controller.jumpCooldown < 0.0f && controller.jumpAnticipate > 0.0f;
	}

	private bool CheckFallConditions()
	{
		return controller.floors.Count <= 0;
	}
}
