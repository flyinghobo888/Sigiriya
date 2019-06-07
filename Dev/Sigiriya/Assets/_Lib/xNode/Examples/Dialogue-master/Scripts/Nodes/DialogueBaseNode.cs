using XNode;

namespace Dialogue {
	public abstract class DialogueBaseNode : XNode.Node
	{
		[Input(backingValue = ShowBackingValue.Never, typeConstraint = TypeConstraint.Inherited)] public DialogueBaseNode input;
		[Output(backingValue = ShowBackingValue.Never)] public DialogueBaseNode output;

		abstract public void Trigger();

		public override object GetValue(NodePort port)
		{
			return null;
		}
	}
}