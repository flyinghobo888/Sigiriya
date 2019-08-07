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
	public EnumMood mood;
	public float moodDuration;
	public bool isVisited = false;
	public bool isNoReturn = false;

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

	//TODO: move this to the graph, since that controls the speed at which lobsters die
	//hmmm but I need the emotion from this node tho
    public Sprite GetSprite(bool isTalking)
    {
        if (speaker)
        {
            return speaker.GetSpriteFromExpression(expression, isTalking);
        }

        //Debug.LogError("THERE IS NO CHARACTER ATTACHED TO THIS NODE! :o");
        return null;
    }
}