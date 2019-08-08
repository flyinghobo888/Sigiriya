using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(46, 217, 125)]
[CreateNodeMenu("Sigiriya/Logic/Access Count")]
public class AccessCount : Node
{
	[Output] public bool result;

	public enum Comparison
	{
		GREATER_THAN,
		LESS_THAN
	}

	public int compareNum;
	public Comparison comparison;

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		SimpleGraph sGraph = graph as SimpleGraph;
		//bool arg1;


		if (comparison == Comparison.GREATER_THAN)
		{
			if (sGraph.timesAccessed > compareNum)
			{
				result = true;
			}
			else
			{
				result = false;
			}
		}
		else
		{
			if (sGraph.timesAccessed < compareNum)
			{
				result = true;
			}
			else
			{
				result = false;
			}
		}

		return result;
	}
}