using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(46, 217, 125)]
[CreateNodeMenu("Logic/Probability")]
public class Probability : Node
{
	[Input(instancePortList = true, connectionType = ConnectionType.Override)] public List<bool> args = new List<bool>();
	[Output] public bool result;

	public int baseProbability = 50;
	public List<int> modVal;

	public void UpdateModLength()
	{
		while (modVal.Count < this.args.Count)
		{
			int num = 0;
			modVal.Add(num);
		}
		int i = modVal.Count;
		while (modVal.Count > this.args.Count)
		{
			modVal.RemoveAt(--i);
		}
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		int calcProb = baseProbability;

		for (int i = 0; i < this.args.Count; i++)
		{
			if (GetInputValue<bool>("args " + i, this.args[i]))
			{
				calcProb += modVal[i];
			}
		}

		int num = Random.Range(0, 101);

		if (num < calcProb)
		{
			result = true;
		}
		else result = false;

		return result;
	}
}