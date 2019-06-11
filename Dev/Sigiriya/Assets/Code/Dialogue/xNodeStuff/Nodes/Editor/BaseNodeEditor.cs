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

		if (node.answers.Count == 0)
		{
			GUILayout.BeginHorizontal();
			NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
			NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
			GUILayout.EndHorizontal();
		}
		else
		{
			NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"));
		}


		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("prompt"), GUIContent.none);
		NodeEditorGUILayout.InstancePortList("answers", typeof(BaseNode), serializedObject, XNode.NodePort.IO.Output, XNode.Node.ConnectionType.Override);
//		GUILayout.BeginHorizontal();
//		EditorGUILayout.LabelField("Flag", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Prompt:")).x));
//		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("dialogueFlag"), GUIContent.none);
//		GUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}

	public override int GetWidth()
	{
		return 300;
	}
}