using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTest : MonoBehaviour
{
	private Rigidbody rb;
	public Rigidbody platform;

	private Vector3 gizmoRadius;

	private float timer = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		if (timer < 0.0f)
		{
			//Vector3 radius = transform.position - platform.transform.position;
			Vector3 radius = (platform.GetPointVelocity(transform.position).magnitude / platform.angularVelocity.magnitude) * Vector3.Cross(platform.GetPointVelocity(transform.position), platform.angularVelocity).normalized;
			Vector3 pivot = transform.position - radius;
			//Vector3 velocity = platform.GetPointVelocity(transform.position);
			//Vector3 destination = transform.position + velocity * Time.deltaTime;
			//gizmoRadius = Quaternion.Euler(platform.angularVelocity * Mathf.Rad2Deg * Time.deltaTime) * radius;

			//accumulatedError += (gizmoRadius.magnitude - radius.magnitude);
			//Debug.Log(accumulatedError);

			transform.RotateAround(pivot, platform.angularVelocity, platform.angularVelocity.magnitude * Mathf.Rad2Deg * Time.deltaTime);
			//transform.position = pivot + gizmoRadius;
			//transform.position = transform.position + platform.GetPointVelocity(transform.position) * Time.deltaTime;
			//transform.position = destination;
			//transform.position = transform.position + ((pivot - transform.position).magnitude - radius.magnitude)*Vector3.Normalize(pivot - transform.position);
			//transform.position = transform.position + (gizmoRadius - radius);
			//transform.position = transform.position + (gizmoRadius - radius) + platform.velocity * Time.deltaTime; //This one?
			//transform.position = transform.position + platform.GetPointVelocity(transform.position) * Time.deltaTime;
		}
		else
		{
			timer -= Time.deltaTime;
		}
	}

	void FixedUpdate()
	{

	}
}
