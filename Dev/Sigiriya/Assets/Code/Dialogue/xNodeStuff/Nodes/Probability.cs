﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeTint(46, 217, 125)]
[CreateNodeMenu("Sigiriya/Logic/Probability")]
public class Probability : Node {

	[Input(typeConstraint = TypeConstraint.Strict)] public bool arg1;
	[Output] public bool result;

	public int probability = 50;
	public int modVal;

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		bool arg1 = GetInputValue<bool>("arg1", this.arg1);

		int calcProb = 0;

		if (arg1)
		{
			calcProb = probability + modVal;
		}
		else
		{
			calcProb = probability;
		}

		int num = Random.Range(0, 100);

		if (num < calcProb)
		{
			result = true;
		}
		else result = false;

		return result;
	}
}