using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DisplayDebugInfo : MonoBehaviour
{
	private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
    }

	void OnDrawGizmos()
	{
		float readPos = transform.position.y - 1.85f;

		//Handles.Label(transform.position + 1.0f * Vector3.up, rb.velocity.magnitude.ToString());
		//Handles.Label(transform.position + 1.0f * Vector3.up, readPos.ToString());
		//Handles.Label(transform.position + 2.5f * Vector3.up, rb.velocity.y.ToString());
	}
}
