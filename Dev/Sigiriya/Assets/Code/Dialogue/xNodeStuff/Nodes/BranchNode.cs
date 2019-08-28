using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(225, 139, 62)]
[CreateNodeMenu("Branch/BranchNode")]
public class BranchNode : BaseNode
{
	[Output(instancePortList = true, connectionType = ConnectionType.Override)] public List<BranchCheck> branches = new List<BranchCheck>();

	public Color connected;
	public void CheckConnectedColor()
	{
		bool isConnected = true;

		if (branches.Count == 0)
		{
			isConnected = false;
		}
		else
		{
			for (int i = 0; i < branches.Count; i++)
			{
				if (GetOutputPort("branches " + i).Connection == null)
				{
					isConnected = false;
				}
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

	public BaseNode GetOutputNode()
	{
		for (int i = 0; i < branches.Count; i++)
		{
			BranchCheck check = GetOutputPort("branches " + i).Connection.node as BranchCheck;

			if (check.CheckBranchFlag())
			{
				BaseNode checkNode = check.GetConnectedNode();

				if (checkNode != null)
				{
					if (checkNode.GetType() == typeof(PromptNode))
					{
						PromptNode pCheckNode = (PromptNode)checkNode;
						if (!pCheckNode.isVisited)
						{
							return checkNode;
						}
					}
					else
					{
						return checkNode;
					}
				}
			}
		}
		
		return GetNextNode();
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null; // Replace this
	}
}