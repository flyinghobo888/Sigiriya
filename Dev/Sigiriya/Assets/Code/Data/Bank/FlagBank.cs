using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlagBank : ManagerBase<FlagBank>
{
	public enum Flags
	{
		NONE,
		_Location,
		swamp,
		hut,
		house,
		kitchen,
		andrewsHouse,
		_Emotion,
		happy,
		sad,
		bigSad,
		angry,
		_Cats,
		longhair,
		shorthair,
		calico,
		andrewsBeauties
	}
	
	//TODO: try to find a way to get rid of this list and just have the flags
	[HideInInspector]
	public static List<Flags> flagList = new List<Flags>
	{
		Flags.NONE,
		Flags._Location,
		Flags.swamp,
		Flags.hut,
		Flags.house,
		Flags.kitchen,
		Flags.andrewsHouse,
		Flags._Emotion,
		Flags.happy,
		Flags.sad,
		Flags.bigSad,
		Flags.angry,
		Flags._Cats,
		Flags.longhair,
		Flags.shorthair,
		Flags.calico,
		Flags.andrewsBeauties
	};

	[HideInInspector]
	public static List<Flags> submenuList = new List<Flags>
	{
		Flags._Location,
		Flags._Emotion,
		Flags._Cats
	};
}