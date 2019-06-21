using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class PromptNode : BaseNode
{
	[Output(instancePortList = true, connectionType = ConnectionType.Override)] public List<BaseNode> responses = new List<BaseNode>();

	[Output] public BaseNode interruptConnection;
	[Output] public BaseNode checkpointConnection;
	[Output] public BaseNode exitConnection;

	[TextArea] public string prompt;
	public tempCharSprites speakerPic;
	public Sprite usedSprite;
	public float time;

	public ResponseNode GetAnswerConnection(int index)
	{
		ResponseNode node = GetOutputPort("responses " + index).Connection.node as ResponseNode;

		return node;
	}

	public PromptNode GetConnectedNode(string portName)
	{
		NodePort port = GetOutputPort(portName);
		if (!port.IsConnected)
		{
            //TODO: I commented this out cuz it was raping the chat
			//Debug.Log(portName + " is not connected!");
			return null;
		}

		PromptNode node = GetOutputPort(portName).Connection.node as PromptNode;
		return node;
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null; // Replace this
	}
}