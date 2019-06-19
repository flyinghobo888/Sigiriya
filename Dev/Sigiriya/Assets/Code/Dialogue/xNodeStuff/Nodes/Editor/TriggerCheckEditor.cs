using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(TriggerCheck))]
public class StringNodeEditor : NodeEditor
{
	public override void OnBodyGUI()
	{
		serializedObject.Update();

		TriggerCheck node = target as TriggerCheck;

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("trigger"), GUIContent.none);
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("operatorType"), GUIContent.none);
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("result"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}

	public override int GetWidth()
	{
		return 150;
	}
}
