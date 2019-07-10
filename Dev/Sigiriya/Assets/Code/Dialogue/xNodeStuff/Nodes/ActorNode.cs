using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class ActorNode : BaseNode
{
	//[Input] BaseNode input;
	//[Output] BaseNode output;

	public enum ActorMovement
	{
		Arriving,
		Leaving
	}

	public ActorMovement status = ActorMovement.Arriving;

	public List<Character> actors = new List<Character>(); 

	// Use this for initialization
	protected override void Init()
	{
		base.Init();
		
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null; // Replace this
	}
}