using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stance", menuName = "Stance")]
public class Stance : ScriptableObject
{
	//TODO: maybe make this abstract,
	//inherit from it, create enums in each with unique members, but same overall names
	//ie.
	/*
	 enum Ting <-name is same
	 {
		1, <-variables unique
		2,
		3
	 }

		 */

	public List<string> stanceNames; //should only ever be 3
	public string currentStance;

	public void SetStance(string newStance)
	{
		if(stanceNames.Contains(newStance))
		{
			currentStance = newStance;
		}
	}

}
