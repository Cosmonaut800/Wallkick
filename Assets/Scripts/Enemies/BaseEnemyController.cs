using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyController : MonoBehaviour
{
	public float health = 100.0f;
	[HideInInspector] public Rigidbody rb;
	public float speed = 10.0f;
	[HideInInspector] public List<Collider> hurtboxes = new List<Collider>();
	public Hurtbox hurtbox;

	protected State currentState;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		currentState = currentState?.RunCurrentState();
	}
}
