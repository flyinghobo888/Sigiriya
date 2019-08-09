using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(FlagNode))]
public class FlagNodeEditor : NodeEditor
{
	FlagEnumHelper flagHelp;// = new FlagEnumHelper();
	FlagBank.Flags m_flag;
	bool canCopy = true;

	public override void OnBodyGUI()
	{
		serializedObject.Update();

		FlagNode node = target as FlagNode;
		SerializedObject so = new SerializedObject(target);

		if (flagHelp == null)
		{
			flagHelp = new FlagEnumHelper();
			flagHelp.InitFlagHelper(canCopy, m_flag);
			Debug.Log("Init FLaghelp");
		}
		flagHelp.CreateMenu(node.throwFlag, out node.throwFlag);
		flagHelp.CopyFlagHelper(out canCopy, out m_flag);

		serializedObject.ApplyModifiedProperties();
	}
}