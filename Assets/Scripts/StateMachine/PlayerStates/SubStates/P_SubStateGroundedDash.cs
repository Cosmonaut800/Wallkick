using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Move the character with the physics engine, turn off ability to lose health for some time in the middle of the dodge animation.
 * Should be interruptable into damage state, jump stateg
 */

public class P_SubStateGroundedDash : SubState
{
	private GameObject player;
	private PlayerController controller;

	public override void Initialize(GameObject parent, State newParentState)
	{
		player = parent;
		parentState = newParentState;
		controller = player.GetComponent<PlayerController>();
	}

	public override SubState RunCurrentSubState()
	{
		throw new System.NotImplementedException();
	}
}
