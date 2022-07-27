using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : StateManager
{
	private GameObject statesParent;

	private void Start()
	{
		statesParent = GameObject.Find("Player");
		currentState = new PlayerStateGrounded();
		currentState.Initialize(statesParent);
	}

	private void Update()
	{
		//I want to run the state machine on FixedUpdate() instead >:)
		//Player states related to movement use physics forces
	}

	private void FixedUpdate()
	{
		RunStateMachine();
		//Debug.Log("CurrentState: "+currentState.ToString());
	}
}
