using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(BaseNode))]
public class BaseNodeEditor : NodeEditor
{
	public override void OnBodyGUI()
	{
		serializedObject.Update();

		BaseNode node = target as BaseNode;

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();

		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("prompt"), GUIContent.none);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("dCon"), GUIContent.none);

		serializedObject.ApplyModifiedProperties();
	}
}