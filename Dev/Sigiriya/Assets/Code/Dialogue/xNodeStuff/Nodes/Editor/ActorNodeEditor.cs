using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(ActorNode))]
public class ActorNodeEditor : NodeEditor
{
	public override void OnBodyGUI()
	{
		serializedObject.Update();

		ActorNode node = target as ActorNode;

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();

		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("status"), GUIContent.none);
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("actors"));

		serializedObject.ApplyModifiedProperties();
	}

	public override int GetWidth()
	{
		return 200;
	}

}
