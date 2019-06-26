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
		_Gayesha,
		gaye_SoftInsult,
		gaye_Incapable,
		gaye_LakminiTime,
		_Lakmini,
		lak_Better,
		lak_Smile,
		lak_Same,
		lak_Worse,
		lak_contribution,
		_BigBitch,
		Its,
		Me,
		Im,
		The,
		Big,
		Bitch
	}

	[HideInInspector]
	public static List<Flags> submenuList = new List<Flags>
	{
		Flags._Chamara,
		Flags._Gayesha,
		Flags._Lakmini,
		Flags._BigBitch
	};
}