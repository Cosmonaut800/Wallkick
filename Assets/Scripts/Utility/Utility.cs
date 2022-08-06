using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
	public const float TIME_EPSILON = 0.01f;

	/*public float[,] GetRotationMatrix(Vector3 reference, Vector3 vector)
	{
		float[,] matrix = new float[3, 3];
		Vector3 angles;

		reference = Vector3.Normalize(reference);
		vector = Vector3.Normalize(vector);

		angles.x = Mathf.Acos(reference.x * vector.x);
		angles.y = Mathf.Acos(reference.y * vector.y);
		angles.z = Mathf.Acos(reference.z * vector.z);



		return matrix;
	}

	public Vector3 VectorMatrixMultiply(Vector3 vector, float[,] matrix)
	{


		return Vector3.zero;
	}*/
}
