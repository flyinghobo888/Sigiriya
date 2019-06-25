using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlagBank : ManagerBase<FlagBank>
{
	public enum Flags
	{
		NONE,
		_Chamara,
		cham_Interest,
		cham_Routine,
		cham_Squabble,
		cham_Tease,
		_SomeoneElse,
		andrew,
		karim,
		dana,
		tim,
		sage,
		allTheArtists,
		_BigBitch,
		Its,
		Me,
		Im,
		The,
		Big,
		Bitch
	}
	
	//TODO: try to find a way to get rid of this list and just have the flags
	[HideInInspector]
	public static List<Flags> flagList = new List<Flags>
	{
		Flags.NONE,
		Flags._Chamara,
		Flags.cham_Interest,
		Flags.cham_Routine,
		Flags.cham_Squabble,
		Flags.cham_Tease,
		Flags._SomeoneElse,
		Flags.andrew,
		Flags.karim,
		Flags.dana,
		Flags.tim,
		Flags.sage,
		Flags.allTheArtists,
		Flags._BigBitch,
		Flags.Its,
		Flags.Me,
		Flags.Im,
		Flags.The,
		Flags.Big,
		Flags.Bitch
	};

	[HideInInspector]
	public static List<Flags> submenuList = new List<Flags>
	{
		Flags._Chamara,
		Flags._SomeoneElse,
		Flags._BigBitch
	};
}