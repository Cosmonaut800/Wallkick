using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
	protected SphereCollider hurtbox;
	public float timer = -1.0f;

	void Start()
	{
		hurtbox = gameObject.AddComponent<SphereCollider>();
		hurtbox.isTrigger = true;
		hurtbox.center = Vector3.zero;
		hurtbox.radius = 0.0f;
		hurtbox.enabled = false;
	}

	void Update()
	{
		timer -= Time.deltaTime;
		if (hurtbox.enabled && timer < 0.0f)
		{
			hurtbox.enabled = false;
		}
	}

	public void Activate(float duration)
	{
		hurtbox.enabled = true;
		timer = duration;
	}

	public void SetRadius(float radius)
	{
		hurtbox.radius = radius;
	}
}
