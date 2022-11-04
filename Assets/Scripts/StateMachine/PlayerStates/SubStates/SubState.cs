using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubState
{
	protected State parentState;

	public abstract SubState RunCurrentSubState();

	public abstract void Initialize(GameObject parent, State newParentState);
}
