using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public string debugString;

	public float health = 100.0f;
	public Image healthBar;

	public float speed = 1.0f;
	public float jumpHeight = 1.0f;

	public float floorThreshold = 0.175f;
	public float ceilingThreshold = 0.175f;

	public float maxFriction = 1.0f;

	private PlayerInput playerInput;
	private float controlTimeCorrection;

	private Rigidbody rb;
	[HideInInspector] public Animator animator;
	
	[HideInInspector] public Vector3 movementVector;
	[HideInInspector] public float angle;
	[HideInInspector] public float usedSpeed;
	[HideInInspector] public RaycastHit hit;

	[HideInInspector] public Rigidbody platform;

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

	[HideInInspector] public bool kick = false;
	[HideInInspector] public int combo = 0;

	private Transform cam;

	// Start is called before the first frame update
	private void Start()
    {
		rb = GetComponent<Rigidbody>();
		animator = transform.Find("Graphics").GetComponent<Animator>();
		usedSpeed = speed;
		cam = transform.Find("Main Camera");
    }

	private void Update()
	{
		healthBar.fillAmount = health / 200.0f;
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

		health += 5.0f * Time.fixedDeltaTime;
		if (health > 100.0f) health = 100.0f;
		else if (health < 0.0f) health = 0.0f;

		//debugString = new Vector2(rb.velocity.x, rb.velocity.z).magnitude.ToString();
		Vector3 forceDir = Quaternion.Euler(0.0f, cam.rotation.eulerAngles.y, 0.0f) * movementVector;
		Vector3 lateralVelocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
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

	public void OnFire(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			kick = true;
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

	public void Damage(float damage)
	{
		health -= damage;
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

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(hit.point, 0.1f);
	}
}
