using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(Not))]
public class NotEditor : NodeEditor
{
	FlagBank.Flags m_flag;
	//TriggerCheck testNode;

	public override void OnBodyGUI()
	{
		serializedObject.Update();

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("arg1"), GUILayout.MinWidth(0));
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("result"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();
	
		serializedObject.ApplyModifiedProperties();
	}

	public override int GetWidth()
	{
		return 70;
	}
}
