using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(46, 217, 125)]
[CreateNodeMenu("Sigiriya/Logic/Conditional")]
public class Conditional : Node
{
	[Input] public bool arg1;
	[Input] public bool arg2;
	[Output] public bool result;

	public Conditionals conditionalType = Conditionals.AND;
	public enum Conditionals
	{
		AND,
		OR
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		bool arg1 = GetInputValue<bool>("arg1", this.arg1);
		bool arg2 = GetInputValue<bool>("arg2", this.arg2);

		if (port.fieldName == "result")
			switch (conditionalType)
			{
				case Conditionals.AND: default: result = arg1 && arg2; break;
				case Conditionals.OR: result = arg1 || arg2; break;
			}
		return result;
	}
}