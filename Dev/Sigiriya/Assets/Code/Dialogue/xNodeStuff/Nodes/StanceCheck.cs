using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(46, 217, 125)]
[CreateNodeMenu("Sigiriya/Logic/StanceCheck")]
public class StanceCheck : Node
{
	public Stance stance;
	public string stanceCheck;

	[Output] public bool result;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		if (stance.currentStance == stanceCheck)
		{
			result = true;
		}
		else
		{
			result = false;
		}

		return result;
	}
}