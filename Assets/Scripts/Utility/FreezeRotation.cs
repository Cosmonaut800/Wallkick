using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
	public Vector3 eulerRotation = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
		transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z);
    }

    // Update is called once per frame
    void LateUpdate()
    {
		transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z);
	}
}
