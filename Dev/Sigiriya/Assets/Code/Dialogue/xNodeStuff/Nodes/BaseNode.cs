using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XNode;

public abstract class BaseNode : XNode.Node
{
	[Input(backingValue = ShowBackingValue.Never, typeConstraint = TypeConstraint.Inherited)] public BaseNode input;
	[Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)] public BaseNode output;

	// Use this for initialization
	protected override void Init()
	{
		base.Init();
	}

	public BaseNode GetNextNode()
	{
		SimpleGraph fmGraph = graph as SimpleGraph;

		NodePort outPort = GetOutputPort("output");

		if (!outPort.IsConnected)
		{
            //Note: Commented this out.
			//Debug.LogWarning("Node isn't connected");
			return null;
		}

		BaseNode node = outPort.Connection.node as BaseNode;
		return node;
	}

	public int GetIndex()
	{
		return graph.nodes.IndexOf(this);
	}

	// Return the correct value of an output port when requested
	//public override object GetValue(NodePort port)
	//{
	//	return null;
	//}
}