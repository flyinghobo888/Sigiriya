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
	public bool isInit = false;

	public List<Character> actors = new List<Character>();
	//TODO: also include a list of emotions that are updated as this goes along

	public void Restart()
	{
		//Find the first DialogueNode without any inputs. This is the starting node.
		current = nodes.Find(x => x is ActorNode && x.Inputs.All(y => !y.IsConnected)) as ActorNode;
		isInit = true;
	}

	public void AddActor(Character newActor)
	{
		if (!actors.Contains(newActor))
		{
			actors.Add(newActor);
		}
	}

	public void RemoveActor(Character oldActor)
	{
		if (actors.Contains(oldActor))
		{
			actors.Remove(oldActor);
		}
	}

	public Sprite GetSprite(bool isTalking, Character actor, Character.EnumExpression expression)
	{
		//TODO: outta bounds error

		if (actors.Contains(actor))
		{
			return actor.GetSpriteFromExpression(expression, isTalking);
			//return actors[index].GetSpriteFromExpression(expression, isTalking);
		}
		else
		{
			Debug.Log("I don't know that actor");
		}

		//Debug.LogError("THERE IS NO CHARACTER ATTACHED TO THIS NODE! :o");
		return null;
	}

	public int GetActorIndex(Character actor)
	{
		return actors.IndexOf(actor);
	}
}