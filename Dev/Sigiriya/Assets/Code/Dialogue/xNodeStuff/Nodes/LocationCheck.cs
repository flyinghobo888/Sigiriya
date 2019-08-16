using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(46, 217, 125)]
[CreateNodeMenu("Logic/LocationCheck")]
public class LocationCheck : Node
{
	[Output] public bool result;
	public EnumLocation location;
	//public EnumMood mood;
	//public EnumTime time;

	[NodeEnum] public Operator operatorType = Operator.TRUE;
	public enum Operator
	{
		TRUE,
		FALSE
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		//TODO: fix persistent event bank to work in editor

		if (port.fieldName == "result")
			switch (operatorType)
			{
				case Operator.TRUE:
				default:
					result = LocationTracker.Instance.TargetLocation == location;
					//result = true;
					break;
				case Operator.FALSE:
					result = LocationTracker.Instance.TargetLocation != location;
					//result = false;
					break;
			}
		return result;
	}
}