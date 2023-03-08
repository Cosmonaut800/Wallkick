using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Add ability to toggle via script so that specific animations can activate this behavior
 */

public class HitboxTrackBone : MonoBehaviour
{
	[SerializeField] private CapsuleCollider hitbox;
	[SerializeField] private Transform targetBone;

	public bool shouldTrack = false;

    // Start is called before the first frame update
    void Start()
    {
		if (shouldTrack) hitbox.center = FindRelativeLateralPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		hitbox.center = Vector3.zero;
		if (shouldTrack) hitbox.center = FindRelativeLateralPosition();
	}

	private Vector3 FindRelativeLateralPosition()
	{
		Vector3 relativePosition = targetBone.position - transform.position;
		relativePosition.y = 0.0f;

		return relativePosition;
	}
}
