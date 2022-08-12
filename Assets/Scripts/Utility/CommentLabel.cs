using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentLabel : MonoBehaviour
{
	public string label;

	[TextArea(2, 16)]
	public string comment;
}
