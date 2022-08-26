using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSlide : MonoBehaviour
{
	public float speed = 1.0f;
	public float distance = 0.0f;
	public Vector3 direction = Vector3.forward;

	private Rigidbody rb;
	private Vector3 startingPosition;
	private float timer = 0.0f;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		startingPosition = transform.position;
	}

	void FixedUpdate()
    {
		timer += Time.fixedDeltaTime;
		rb.MovePosition(startingPosition + distance * Mathf.Sin(timer * speed) * direction);
    }
}
