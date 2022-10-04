using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAerial : State
{
	private GameObject player;
	private PlayerController controller;
	private Transform graphics;
	private Transform cam;
	private Rigidbody rb;

	private Vector3 lastVelocity;
	private Vector3 lastLastVelocity;

	private Vector3 lateralVelocity;

	private static float WALLKICK_VELOCITY = 7.5f;

	public override void Initialize(GameObject parent)
	{
		player = parent;
		controller = player.GetComponent<PlayerController>();
		graphics = player.transform.Find("Graphics");
		cam = player.transform.Find("Main Camera");
		rb = controller.GetComponent<Rigidbody>();
		rb.drag = 0.0f;
		controller.animator.SetTrigger("PlayerState.Aerial");  // Animator
	}

	public override State RunCurrentState()
	{
		// Animator
		controller.animator.SetFloat(
			"Aerial.Jump",
			Mathf.Clamp01(-0.05f * rb.velocity.y + 0.5f)
		);

		Vector3 forceDir = Quaternion.Euler(0.0f, cam.rotation.eulerAngles.y, 0.0f) * controller.movementVector;
		float coefficient = 1.0f;

		//Limit moving force when travelling above speed limit
		lateralVelocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
		if (lateralVelocity.magnitude > 10.0f)
		{
			coefficient = 1.0f - ((Vector3.Dot(lateralVelocity.normalized, forceDir) + 1.0f) / 2.0f);
		}

		//Moving force
		rb.AddForce(coefficient * 0.1f * controller.speed * forceDir);

		//Shorten jump when the jump button is released
		if (controller.doJump && controller.touchedGround)
		{
			if (controller.prolongJump > 0.0f)
			{
				rb.AddForce(-5.0f * Physics.gravity, ForceMode.Acceleration);

				controller.prolongJump -= Time.fixedDeltaTime;
			}
			else if (rb.velocity.y < 15.0f)
			{
				controller.shortenJump = true;
			}
		}

		if (controller.shortenJump)
		{
			rb.AddForce(1.0f * Physics.gravity, ForceMode.Acceleration);
			controller.touchedGround = false;
		}

		//Get last airspeed before colliding with a wall, for walljumps
		if (controller.walls.Count <= 0)
		{
			lastLastVelocity = lastVelocity; //For some reason one frame of velocity is recorded after colliding with the wall, so we need the velocity from 2 frames ago.
			lastVelocity = rb.velocity;
		}

		//Perform substate action
		if (controller.kick)
		{
			controller.kick = false;

			if (controller.combo < 1)
			{
				controller.animator.SetTrigger("Kick");
				controller.combo++;
			}
		}

		if (controller.colliding > 0 && controller.jumpCooldown < 0.0f)
		{
			//Touching the floor -> Grounded
			if (CheckLandingConditions())
			{
				PlayerStateGrounded nextState = new PlayerStateGrounded();

				controller.shortenJump = false;

				nextState.Initialize(player);
				return nextState;
			}
			//Touching a wall -> Aerial
			else if (CheckWallkickConditions())
			{
				Vector3 wallNormal = AverageWalls(controller.walls);
				Vector3 force = -Vector3.Dot(lastLastVelocity, wallNormal) * wallNormal;

				if(force.magnitude < WALLKICK_VELOCITY/2.0f)
				{
					force = WALLKICK_VELOCITY/2.0f * wallNormal; //Add constant horizontal component
				}
				if (rb.velocity.y < WALLKICK_VELOCITY*2.0f)
				{
					force.y = WALLKICK_VELOCITY*2.0f - rb.velocity.y; //Add constant vertical component
				}
				else
				{
					force.y += WALLKICK_VELOCITY/2.0f - rb.velocity.y;
				}

				controller.jumpCooldown = 0.2f + Utility.TIME_EPSILON;

				controller.touchedGround = true;
				controller.prolongJump = -1.0f;
				controller.shortenJump = false;

				rb.AddForce(force, ForceMode.VelocityChange);

				graphics.rotation = Quaternion.Euler(0.0f, Mathf.Atan2(rb.velocity.x + force.x, rb.velocity.z + force.z) * Mathf.Rad2Deg, 0.0f);
			}
		}

		return this;
	}

	private Vector3 AverageWalls(List<ContactPoint> walls)
	{
		Vector3 result = Vector3.zero;

		for (int i = 0; i < walls.Count; i++)
		{
			result = result + walls[i].normal;
		}

		result = Vector3.Normalize(result);

		return result;
	}

	//----State Transitions----------------
	private bool CheckLandingConditions()
	{
		return controller.floors.Count > 0;
	}

	private bool CheckWallkickConditions()
	{
		return (controller.walls.Count > 0) && (controller.jumpCooldown < 0.0f && controller.jumpAnticipate > 0.0f) && controller.wallHitTimer > 0.0f;
	}
}
