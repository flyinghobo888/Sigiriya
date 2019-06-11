﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BaseNode : XNode.Node
{
	[Input(backingValue = ShowBackingValue.Never, typeConstraint = TypeConstraint.Inherited)] public BaseNode input;
	[Output(backingValue = ShowBackingValue.Never)] public BaseNode output;
	[Output(instancePortList = true)] public List<Answer> answers = new List<Answer>();

	[TextArea] public string prompt;

	[System.Serializable]
	public class Answer
	{
		public string text;
		public AudioClip voiceClip;
		public bool isHidden;
		public string throwFlag;
		public string catchFlag;
	}

	// Use this for initialization
	protected override void Init()
	{
		base.Init();
	}

	public BaseNode GetNextNode()
	{
		SimpleGraph fmGraph = graph as SimpleGraph;

		NodePort exitPort = GetOutputPort("output");

		if (!exitPort.IsConnected)
		{
			Debug.LogWarning("Node isn't connected");
			return null;
		}

		BaseNode node = exitPort.Connection.node as BaseNode;
		return node;
	}

	public BaseNode GetAnswerConnection(int index)
	{
		BaseNode node = GetOutputPort("answers " + index).Connection.node as BaseNode;

		return node;
	}

	public int GetIndex()
	{
		return graph.nodes.IndexOf(this);
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null;
	}
}