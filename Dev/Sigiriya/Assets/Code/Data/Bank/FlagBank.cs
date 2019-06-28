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
		Flags._Gayesha,
		Flags.gaye_SoftInsult,
		Flags.gaye_Incapable,
		Flags.gaye_LakminiTime,
		Flags._Lakmini,
		Flags.lak_Better,
		Flags.lak_Smile,
		Flags.lak_Same,
		Flags.lak_Worse,
		Flags.lak_contribution,
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
		Flags._Gayesha,
		Flags._Lakmini,
		Flags._BigBitch
	};
}