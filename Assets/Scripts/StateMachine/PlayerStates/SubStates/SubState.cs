using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SubState
{
	public abstract SubState RunCurrentSubState();

	public abstract void Initialize(GameObject parent);
}
