using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
	public State nextState;

	public abstract State RunCurrentState();

	public abstract void Initialize(GameObject parent);
}
