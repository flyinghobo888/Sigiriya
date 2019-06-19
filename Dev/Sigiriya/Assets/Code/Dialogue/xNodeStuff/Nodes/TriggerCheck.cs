using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TriggerCheck : Node
{
	[Output] public bool result;
	public bool trigger;

	public Operator operatorType = Operator.TRUE;
	public enum Operator
	{
		TRUE,
		FALSE
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		//TODO: check if the trigger exists in the bank of active triggers

		if (port.fieldName == "result")
			switch (operatorType)
			{
				case Operator.TRUE: default: result = trigger==true ; break;
				case Operator.FALSE: result = trigger==false; break;
			}
		return result;
	}
}