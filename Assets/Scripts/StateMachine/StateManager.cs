using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
	protected State currentState;

    // Update is called once per frame
    void Update()
    {
		Debug.Log("StateMachine Update()");
		RunStateMachine();
    }

	protected virtual void RunStateMachine()
	{
		State nextState = currentState?.RunCurrentState();

		if(nextState != null)
		{
			SwitchToNextState(nextState);
		}
	}

	protected virtual void SwitchToNextState(State nextState)
	{
		currentState = nextState;
	}
}
