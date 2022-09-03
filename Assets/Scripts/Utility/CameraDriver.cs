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
	private CinemachinePOV camPOV;
	[SerializeField] private PlayerInput playerInput;

	private float controlTimeCorrection = 1.0f;

    // Start is called before the first frame update
    void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		playerInput.controlsChangedEvent.AddListener(OnChangedControls);

		cam = transform.Find("Camera Pivot/CM vcam1").GetComponent<CinemachineVirtualCamera>();
		camPOV = cam.GetCinemachineComponent<CinemachinePOV>();
    }

	void Update()
	{
		//camPivot.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
	}

	private void OnChangedControls(PlayerInput arg0) //Don't understand where this syntax comes from at all :D
	{
		if (playerInput.currentControlScheme == "Gamepad")
		{
			controlTimeCorrection = Time.deltaTime;
		}
		else
		{
			controlTimeCorrection = 1.0f;
		}
	}

	public void OnLook(InputAction.CallbackContext context)
	{
		Vector2 inputLook = context.ReadValue<Vector2>();
		
		camPOV.m_HorizontalAxis.m_InputAxisValue = controlTimeCorrection * inputLook.x;
		camPOV.m_VerticalAxis.m_InputAxisValue = controlTimeCorrection * inputLook.y;
		Debug.Log(controlTimeCorrection);
	}
}
