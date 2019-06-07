using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BaseNode : XNode.Node
{
	[Input(backingValue = ShowBackingValue.Never)] public BaseNode input;
	[Output] public BaseNode output;

	[TextArea] public string prompt;

	public DialogueController dCon;

	// Use this for initialization
	protected override void Init()
	{
		base.Init();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null;
	}
}