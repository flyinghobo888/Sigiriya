using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TriggerCheck : Node
{
	[Output] public bool result;
	public FlagBank.Flags trigger;


	public Operator operatorType = Operator.TRUE;
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
				case Operator.TRUE: default:
					result = PersistentEventBank.ContainsFlag(trigger) == true ;
					//result = true;
					break;
				case Operator.FALSE:
					result = PersistentEventBank.ContainsFlag(trigger) == false;
					//result = false;
					break;
			}
		return result;
	}
}