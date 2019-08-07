﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
[NodeTint(46, 217, 125)]
[CreateNodeMenu("Sigiriya/Logic/TimeCheck")]
public class TimeCheck : Node
{
	[Output] public bool result;
	public Character character;
	public EnumTime time;

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
					result = GlobalTimeTracker.Instance.CurrentTimeOfDay == time;
					//result = true;
					break;
				case Operator.FALSE:
					result = GlobalTimeTracker.Instance.CurrentTimeOfDay != time;
					//result = false;
					break;
			}
		return result;
	}
}