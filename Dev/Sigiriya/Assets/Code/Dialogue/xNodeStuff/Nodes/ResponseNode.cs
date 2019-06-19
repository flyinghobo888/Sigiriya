using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class ResponseNode : BaseNode
{
	[Input(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)] public bool isHidden;

	public string text;
	public AudioClip voiceClip;
	public string throwFlag;

//	public void CheckFlag(string flag)
//	{
//		isHidden = this.GetInputValue<bool>("isHidden", this.isHidden);	
//	}

	public bool getHidden()
	{
		isHidden = this.GetInputValue<bool>("isHidden", this.isHidden);
		return isHidden;
	}


	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}