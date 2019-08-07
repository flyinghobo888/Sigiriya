using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(225, 139, 62)]
[CreateNodeMenu("Sigiriya/Branch/BranchCheck")]
public class BranchCheck : Node
{
	[Input] public BranchCheck input;
	[Input] public bool flag = true;
	[Output] public BaseNode output;

	public bool CheckBranchFlag()
	{
		bool flag = GetInputValue<bool>("flag", this.flag);

		return flag;
	}

	public BaseNode GetConnectedNode()
	{
		SimpleGraph fmGraph = graph as SimpleGraph;

		NodePort outPort = GetOutputPort("output");

		if (!outPort.IsConnected)
		{
			//TODO: Commented this out.
			Debug.LogWarning("Branch goes nowhere!");
			return null;
		}

		BaseNode node = outPort.Connection.node as BaseNode;
		return node;
	}


	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null; // Replace this
	}
}