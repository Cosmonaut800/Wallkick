using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpin : MonoBehaviour
{
	public Vector3 pivot = Vector3.zero;
	public bool clockwise = true;
	public float speed = 0.0f;
	public Rigidbody rb;

	private float spinAngle = 0.0f;
	private Vector3 startingPos;

	void Start()
	{
		startingPos = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		float direction = 1.0f;
		Quaternion rot = Quaternion.Euler(0.0f, spinAngle, 0.0f);

		if (!clockwise)
		{
			direction = -1.0f;
		}
		spinAngle += direction * speed * Time.fixedDeltaTime;
		rb.MoveRotation(rot);
		rb.MovePosition(startingPos + pivot - (rot * pivot));

		if (spinAngle > 360.0f) spinAngle -= 360.0f;
    }
}
