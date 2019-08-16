using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(189, 169, 30)]
[CreateNodeMenu("Dialogue/Response")]
public class ResponseNode : BaseNode
{
	[Input(typeConstraint = TypeConstraint.Strict)] ResponseNode promptInput;
	[Input(connectionType = ConnectionType.Override)] public bool isHidden;

	[TextArea(8, 8)] public string textFull;
	public string textButton;
	public AudioClip voiceClip;
	public FlagBank.Flags throwFlag;

	public bool getHidden()
	{
		NodePort port = GetInputPort("isHidden");
		if (port.IsConnected)
		{
			isHidden = !this.GetInputValue<bool>("isHidden", this.isHidden);
		}
		else
		{
			isHidden = false;
		}
		return isHidden;
	}


	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}