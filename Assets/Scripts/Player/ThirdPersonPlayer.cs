using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonPlayer : MonoBehaviour
{
	public float speed = 1.0f;
	public float jumpHeight = 1.0f;

	public float floorThreshold = 0.175f;
	public float ceilingThreshold = 0.175f;

	private Rigidbody rb;
	[HideInInspector] public Animator animator;
	
	[HideInInspector] public Vector3 movementVector;
	[HideInInspector] public float usedSpeed;

	[HideInInspector] public float jumpAnticipate = -Utility.TIME_EPSILON;
	[HideInInspector] public float jumpCooldown = 0.02f;
	[HideInInspector] public bool doJump = false;
	[HideInInspector] public float prolongJump = 0.0f;
	[HideInInspector] public bool shortenJump = false;
	[HideInInspector] public bool touchedGround = true;

	private bool physicsProcessed = false;
	[HideInInspector] public int colliding = 0;
	[HideInInspector] public List<ContactPoint> floors = new List<ContactPoint>();
	[HideInInspector] public List<ContactPoint> ceilings = new List<ContactPoint>();
	[HideInInspector] public List<ContactPoint> walls = new List<ContactPoint>();

	private int lastWallsTouched = 0;
	[HideInInspector] public float wallHitTimer = 0.0f;

	// Start is called before the first frame update
	private void Start()
    {
		rb = GetComponent<Rigidbody>();
		animator = transform.Find("Graphics").GetComponent<Animator>();
		usedSpeed = speed;
    }

	// Update is called once per frame
	private void FixedUpdate()
    {
		jumpCooldown -= Time.fixedDeltaTime;
		jumpAnticipate -= Time.fixedDeltaTime;

		wallHitTimer -= Time.fixedDeltaTime;
		if (walls.Count > lastWallsTouched)
		{
			wallHitTimer = 0.3f;
		}
		lastWallsTouched = walls.Count;

		physicsProcessed = false;
	}

	private void OnCollisionEnter(Collision collision)
	{
		colliding++;
	}

	private void OnCollisionStay(Collision collision)
	{
		SortContacts(collision);
	}

	private void OnCollisionExit(Collision collision)
	{
		colliding--;
		ClearContacts();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		Vector2 inputMovement = context.ReadValue<Vector2>();
		movementVector = new Vector3(inputMovement.x, 0, inputMovement.y);
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			jumpAnticipate = 0.1f;
			doJump = true;
			prolongJump = 0.1f;
		}

		if (context.canceled)
		{
			doJump = false;
			shortenJump = true;
		}
	}

	private void SortContacts(Collision collision)
	{
		if (!physicsProcessed)
		{
			physicsProcessed = true;
			ClearContacts();
		}

		for (int i = 0 ; i < collision.contactCount; i++)
		{
			if (collision.contacts[i].normal.magnitude < 0.5f)
			{
				Debug.LogWarning("Contact point with normal of length zero retrieved.");
			}
			else if (Vector3.Dot(collision.contacts[i].normal, Vector3.up) > floorThreshold)
			{
				floors.Add(collision.contacts[i]);
			}
			else if (Vector3.Dot(collision.contacts[i].normal, Vector3.down) > ceilingThreshold)
			{
				ceilings.Add(collision.contacts[i]);
			}
			else
			{
				walls.Add(collision.contacts[i]);
			}
		}
	}

	private void ClearContacts()
	{
		floors.Clear();
		ceilings.Clear();
		walls.Clear();
	}

	private void OnDrawGizmos()
	{
		for (int i = 0; i < floors.Count; i++)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawRay(floors[i].point, floors[i].normal);
		}
		for (int i = 0; i < ceilings.Count; i++)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(ceilings[i].point, ceilings[i].normal);
		}
		for (int i = 0; i < walls.Count; i++)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawRay(walls[i].point, walls[i].normal);
		}
	}
}
