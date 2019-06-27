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
		_Isuri,
		isu_Busy,
		isu_SaidLeave,
		isu_Bother,
		isu_Childish,
		isu_Left,
		isu_Angry,
		_Neja,
		neja_map,
		neja_momDead,
		neja_scarab,
		_Kalathma,
		kal_talkWife,
		Kal_weirdAdvice,
		kal_worry,
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
		Flags._Isuri,
		Flags._Neja,
		Flags._Kalathma,
		Flags._BigBitch
	};
}