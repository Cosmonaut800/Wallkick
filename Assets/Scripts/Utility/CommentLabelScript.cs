using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentLabelScript : StateMachineBehaviour
{
	public string label;

	[TextArea(2, 16)]
	public string comment;
}
