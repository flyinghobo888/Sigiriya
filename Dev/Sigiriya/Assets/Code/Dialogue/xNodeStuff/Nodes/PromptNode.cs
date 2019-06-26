using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Sigiriya/Dialogue/PromptNode")]
public class PromptNode : BaseNode
{
	[Output(instancePortList = true, connectionType = ConnectionType.Override)] public List<BaseNode> responses = new List<BaseNode>();

	[Output(connectionType = ConnectionType.Override)] public BaseNode interruptConnection;
	[Output(connectionType = ConnectionType.Override)] public BaseNode checkpointConnection;
	[Output(connectionType = ConnectionType.Override)] public BaseNode exitConnection;

	[TextArea] public string prompt;
    public Character speaker;
	public Character.EnumExpression expression;
	public float time;

	public ResponseNode GetAnswerConnection(int index)
	{
		ResponseNode node = GetOutputPort("responses " + index).Connection.node as ResponseNode;

		return node;
	}

	public BaseNode GetConnectedNode(string portName)
	{
		NodePort port = GetOutputPort(portName);
		if (!port.IsConnected)
		{
            //TODO: I commented this out cuz it was raping the chat
			//Debug.Log(portName + " is not connected!");
			return null;
		}

		BaseNode node = GetOutputPort(portName).Connection.node as BaseNode;
		return node;
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null; // Replace this
	}

    public Sprite GetSprite(bool isTalking)
    {
        if (speaker)
        {
            return speaker.GetSpriteFromExpression(expression, isTalking);
        }

        Debug.LogError("THERE IS NO CHARACTER ATTACHED TO THIS NODE! :o");
        return null;
    }
}