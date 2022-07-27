using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraDriver : MonoBehaviour
{
	[Tooltip("Multiplier for gamepad input.")] public float xMultiplier = 1.0f;
	[Tooltip("Multiplier for gamepad input.")] public float yMultiplier = 1.0f;

	private CinemachineVirtualCamera cam;
	private Transform camPivot;
	private CinemachinePOV camPOV;
	[SerializeField] private PlayerInput player;

	private Vector2 gizmoLook;

    // Start is called before the first frame update
    void Start()
    {
		cam = transform.Find("Camera Pivot/CM vcam1").GetComponent<CinemachineVirtualCamera>();
		camPivot = transform.Find("Camera Pivot");
		camPOV = cam.GetCinemachineComponent<CinemachinePOV>();
    }

	void Update()
	{
		//camPivot.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		Vector2 inputLook = context.ReadValue<Vector2>();
		gizmoLook = inputLook;
		float xMult = 1.0f;
		float yMult = 1.0f;

		if (player.currentControlScheme == "Gamepad")
		{
			xMult = xMultiplier;
			yMult = yMultiplier;
		}
		
		camPOV.m_HorizontalAxis.m_InputAxisValue = inputLook.x * xMult;
		camPOV.m_VerticalAxis.m_InputAxisValue = inputLook.y * yMult;
	}
}
