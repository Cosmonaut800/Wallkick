using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DisplayDebugInfo : MonoBehaviour
{
	public Rigidbody rb;
	public PlayerStateManager playerStateManager;

    // Start is called before the first frame update
    void Start()
    {
		
    }

	void OnDrawGizmos()
	{
		float readPos = transform.position.y - 1.85f;

		//Handles.Label(transform.position + 1.0f * Vector3.up, playerStateManager.currentState.ToString());
		//Handles.Label(transform.position + 1.0f * Vector3.up, rb.velocity.magnitude.ToString());
		//Handles.Label(transform.position + 1.0f * Vector3.up, readPos.ToString());
		//Handles.Label(transform.position + 2.5f * Vector3.up, rb.velocity.y.ToString());
	}
}
