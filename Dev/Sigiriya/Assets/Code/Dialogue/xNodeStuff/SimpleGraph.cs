using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using XNode;

[CreateAssetMenu]
public class SimpleGraph : NodeGraph
{
	//TODO: add numberOfTimes logic node

	[HideInInspector]
	public BaseNode current;
	public bool isInit = false;
	public int timesAccessed = 0;

	public List<Character> actors = new List<Character>();

	//Dialogue pool variables
	public List<EnumLocation> location;
	public List<FlagBank.Flags> neededFlags;
	public List<EnumTime> timeOfDay;
	public List<SimpleGraph> dependantConversation;
	//recently talked, but not finished //Check by checking the isInit

	public bool isDone = false;

	public void Reset()
	{
		isInit = false;
		isDone = false;
		timesAccessed = 0;
		ResetIsVisited();
	}

	public void Init()
	{
		//Find the first Dialogue ActorNode without any inputs. This is the starting node.
		current = nodes.Find(x => x is ActorNode && x.Inputs.All(y => !y.IsConnected)) as ActorNode;
		timesAccessed = 0;
		ResetIsVisited();

		isInit = true;
	}

	public void ResetIsVisited()
	{
		for (int i = 0; i < nodes.Count; i++)
		{
			if (nodes[i] is PromptNode)
			{
				PromptNode pNode = nodes[i] as PromptNode;

				pNode.isVisited = false;
			}
		}

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
			//Debug.Log("I don't know that actor");
		}

		//Debug.LogError("THERE IS NO CHARACTER ATTACHED TO THIS NODE! :o");
		return null;
	}

	public int GetActorIndex(Character actor)
	{
		return actors.IndexOf(actor);
	}
}