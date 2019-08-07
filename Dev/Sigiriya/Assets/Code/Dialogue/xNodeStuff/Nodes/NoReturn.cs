using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class NoReturn : BaseNode
{
	//This node does nothing for now. Just gets read by controller to signify it was hit

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}