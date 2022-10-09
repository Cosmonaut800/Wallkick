using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyController : BaseEnemyController
{
	public Transform target;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		currentState = new TestEnemyDefaultState();
		currentState.Initialize(gameObject);
	}
}
