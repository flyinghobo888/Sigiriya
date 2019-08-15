using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Dialogue/PromptNode")]
public class PromptNode : BaseNode
{
	[Output(instancePortList = true, connectionType = ConnectionType.Override)] public List<BaseNode> responses = new List<BaseNode>();

	[Output(connectionType = ConnectionType.Override)] public BaseNode interruptConnection;
	[Output(connectionType = ConnectionType.Override)] public BaseNode checkpointConnection;
	[Output(connectionType = ConnectionType.Override)] public BaseNode exitConnection;

	[TextArea(3,8)] public string prompt;
    public Character speaker;
	public Character.EnumExpression expression;
	public EnumMood mood;

	//SigiTime variables
	public int sec = 0, min = 0, hr = 0, dys = 0;

    public bool isVisited = false;
	public bool isNoReturn = false;

	//TODO: fix stances later
	public string newStance;

	public Color connected;
	public void CheckConnectedColor()
	{
		bool isConnected = true;

		if (GetConnectedNode("output") == null)
		{
			isConnected = false;
		}

		if (responses.Count > 0)
		{
			isConnected = true;
		}

		for (int i = 0; i < responses.Count; i++)
		{
			if (GetAnswerConnection(i) == null)
			{
				isConnected = false;
			}
		}

		if (isConnected)
		{
			connected = Color.green;
		}
		else
		{
			connected = Color.red;
		}
	}

	public ResponseNode GetAnswerConnection(int index)
	{
		if (GetOutputPort("responses " + index).Connection != null)
		{
			ResponseNode node = GetOutputPort("responses " + index).Connection.node as ResponseNode;
			return node;
		}
		return null;
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