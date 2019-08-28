using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(BranchCheck))]
public class BranchCheckEditor : NodeEditor
{
	public override void OnBodyGUI()
	{
		serializedObject.Update();

		BranchCheck node = target as BranchCheck;

		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("flag"), GUILayout.MinWidth(0));

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
		node.CheckConnectedColor();
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("connected"), GUIContent.none);
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}
}
