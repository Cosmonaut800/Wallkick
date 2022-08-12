using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DisplayDebugInfo : MonoBehaviour
{
	public Rigidbody rb;
	public PlayerStateManager playerStateManager;
	public PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
		
    }

	void OnDrawGizmos()
	{
		float readPos = transform.position.y - 1.85f;

		Handles.Label(transform.position + 1.0f * Vector3.up, controller.debugString);
	}
}
