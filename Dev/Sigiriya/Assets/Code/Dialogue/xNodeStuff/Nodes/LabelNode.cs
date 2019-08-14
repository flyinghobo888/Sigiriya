using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(255,0,183)]
[CreateNodeMenu("Sigiriya/Label")]
public class Label : Node
{
	public string label;

	// Use this for initialization
	protected override void Init()
	{
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null; // Replace this
	}
}