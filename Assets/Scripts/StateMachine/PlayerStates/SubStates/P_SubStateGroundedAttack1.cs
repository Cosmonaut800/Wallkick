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
	//private bool exitedIdleRun = false;
	private int transitionCount = 0;

	public override void Initialize(GameObject parent, State newParentState)
	{
		player = parent;
		parentState = newParentState;
		controller = player.GetComponent<PlayerController>();
		controller.animator.SetTrigger("Attack");
	}

	public override SubState RunCurrentSubState()
	{
		if (controller.animator.IsInTransition(0))
		{
			transitionCount++;
		}

		if (transitionCount > 0 && controller.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
		{
			P_SubStateGroundedIdle nextSubState = new P_SubStateGroundedIdle();
			nextSubState.Initialize(player, parentState);
			Debug.Log("Attack exit");
			return nextSubState;
		}

		return this;
	}
}
