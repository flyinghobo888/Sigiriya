using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlagBank : ManagerBase<FlagBank>
{
	public enum Flags
	{
		NONE,
		_Toms_Flags,
		Trust,
		Selling_Something,
		Chamara_Works,
		Asked_Gayesha_To_Cook,
		Lakmini_Is_Welcome,
		Learned_Truth,
		Told_Truth,
		Comforted_Gayesha,
		Saw_Pamu_Drunk,
		_Test_Flags,
		Alpha,
		Bravo,
		Charlie,
		Dick
	}

	[HideInInspector]
	public static List<Flags> submenuList = new List<Flags>
	{
		FlagBank.Flags._Toms_Flags,
		FlagBank.Flags._Test_Flags
	};
}