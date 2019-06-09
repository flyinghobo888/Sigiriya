using UnityEngine;
using XNode;

namespace Dialogue {
	public abstract class DialogueBaseNode : XNode.Node
	{
		[Input(backingValue = ShowBackingValue.Always, typeConstraint = TypeConstraint.Inherited)] public DialogueBaseNode input;
		[Output(backingValue = ShowBackingValue.Always)] public DialogueBaseNode output;

		abstract public void Trigger();

		public DialogueBaseNode GetNextNode()
		{
			DialogueGraph fmGraph = graph as DialogueGraph;

			NodePort exitPort = GetOutputPort("output");

			if (!exitPort.IsConnected)
			{
				Debug.LogWarning("Node isn't connected");
				return null;
			}

			DialogueBaseNode node = exitPort.Connection.node as DialogueBaseNode;
			return node;
		}

		public int GetIndex()
		{
			return graph.nodes.IndexOf(this);
		}

		public override object GetValue(NodePort port)
		{
			return null;
		}
	}
}