using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using XNode;

[CreateAssetMenu]
public class SimpleGraph : NodeGraph
{
	[HideInInspector]
	public BaseNode current;

	public void Restart()
	{
		//Find the first DialogueNode without any inputs. This is the starting node.
		current = nodes.Find(x => x is BaseNode && x.Inputs.All(y => !y.IsConnected)) as BaseNode;
	}
}