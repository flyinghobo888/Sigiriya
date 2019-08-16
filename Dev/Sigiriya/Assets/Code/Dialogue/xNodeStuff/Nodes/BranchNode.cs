using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(225, 139, 62)]
[CreateNodeMenu("Branch/BranchNode")]
public class BranchNode : BaseNode
{
	/*TODO: This class will accept a base node, as well as a bunch of 
			Bool inputs from triggers. Depending on the trigger*, that 
			connection will be the output. If none pass, the output 
			is the default

		*I couldn't figure out a way to have dynamic input AND output pins.
		* *actually, possible solution. treat it like responses. connecto to responses. but branch versions, not actual responses
	*/

	[Output(instancePortList = true, connectionType = ConnectionType.Override)] public List<BranchCheck> branches = new List<BranchCheck>();

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