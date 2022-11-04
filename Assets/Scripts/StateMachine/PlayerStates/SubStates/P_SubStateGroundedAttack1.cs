using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_SubStateGroundedAttack1 : SubState
{
	/*
	 * Make so the substate does not exit until current attacking animation finishes
	 */

	private GameObject player;
	private PlayerController controller;

	public override void Initialize(GameObject parent, State newParentState)
	{
		player = parent;
		parentState = newParentState;
		controller = player.GetComponent<PlayerController>();
		controller.animator.SetTrigger("Attack");
	}

	public override SubState RunCurrentSubState()
	{
		P_SubStateGroundedIdle nextSubState = new P_SubStateGroundedIdle();
		nextSubState.Initialize(player, parentState);

		Debug.Log("Attack executed");

		return nextSubState;
	}
}
