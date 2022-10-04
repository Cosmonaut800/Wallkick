using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFPS : MonoBehaviour
{
	// Start is called before the first frame update

	[SerializeField] private bool vSync = true;
	[SerializeField] private int framerate = 60;
	[SerializeField] private int vSyncCount = 1;

	private int lastFrameRate = 60;

	private void Start()
	{
		if (vSync)
		{
			QualitySettings.vSyncCount = vSyncCount;
		}
		else
		{
			Application.targetFrameRate = framerate;
		}

		Cursor.lockState = CursorLockMode.Locked;

		lastFrameRate = framerate;
	}

	private void Update()
	{
		if (framerate != lastFrameRate)
		{
			Application.targetFrameRate = framerate;
			lastFrameRate = framerate;
		}
	}
}
