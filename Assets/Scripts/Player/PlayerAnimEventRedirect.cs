using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventRedirect : MonoBehaviour
{
	[SerializeField] private HitboxTrackBone tracker;
	[SerializeField] private Hurtbox hurtbox;

	public void HitboxTrackBone(int value)
	{
		if (value == 0)
		{
			tracker.shouldTrack = false;
		}
		else
		{
			tracker.shouldTrack = true;
		}	
	}

	public void HurtboxChangeRadius(float value)
	{
		hurtbox.SetRadius(value);
	}

	public void HurtboxActivate(float duration)
	{
		hurtbox.Activate(duration);
	}
}
