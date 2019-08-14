using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("Sigiriya/Dialogue/ResponseBranch")]
public class ResponseBranchNode : BaseNode
{
	[Output(instancePortList = true, connectionType = ConnectionType.Override)] public List<BaseNode> responses = new List<BaseNode>();

	public Color connected;
	public void CheckConnectedColor()
	{
		bool isConnected = true;

		if (GetConnectedNode("output") == null)
		{
			//isConnected = false;
		}

		if (responses.Count == 0)
		{
			isConnected = false;
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
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}