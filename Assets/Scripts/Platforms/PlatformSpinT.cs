using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpinT : MonoBehaviour
{
	public Vector3 pivot = Vector3.zero;
	public bool clockwise = true;
	public float speed = 0.0f;
	public Rigidbody rb;

	private float spinAngle = 0.0f;
	private Vector3 startingPosition;

	void Start()
	{
		startingPosition = transform.position;
	}

	void Update()
	{
		float direction = 1.0f;
		Quaternion rot = Quaternion.Euler(0.0f, spinAngle, 0.0f);

		if (!clockwise)
		{
			direction = -1.0f;
		}
		spinAngle += direction * speed * Time.deltaTime;
		transform.rotation = Quaternion.Euler(0.0f, spinAngle, 0.0f);
		transform.position = startingPosition + pivot - (rot * pivot);

		if (spinAngle > 360.0f) spinAngle -= 360.0f;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(rb.position + (Quaternion.Euler(0.0f, spinAngle, 0.0f) * pivot), Vector3.Normalize(rb.angularVelocity));
	}
}
