using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(BaseNode))]
public class BaseNodeEditor : NodeEditor
{
	public bool openFoldout;

	public override void OnBodyGUI()
	{
		serializedObject.Update();

		BaseNode node = target as BaseNode;

//		if (node.answers.Count == 0)
//		{
			GUILayout.BeginHorizontal();
			NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
			NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
			GUILayout.EndHorizontal();
//		}
//		else
//		{
//			NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"));
//		}


		GUILayout.BeginVertical();
		openFoldout = EditorGUILayout.Foldout(openFoldout, "Extra Connections:", EditorStyles.boldFont);
		if (openFoldout)
		{
			//TODO: maybe try to color these?
			NodeEditorGUILayout.PortField(new GUIContent("Interrupt Connection "), target.GetOutputPort("interruptConnection"), GUILayout.MinWidth(0));
			NodeEditorGUILayout.PortField(new GUIContent("Checkpoint Connection "), target.GetOutputPort("checkPointConnection"), GUILayout.MinWidth(0));
			NodeEditorGUILayout.PortField(new GUIContent("Exit Connection "), target.GetOutputPort("exitConnection"), GUILayout.MinWidth(0));
			//TODO: call this in window editor instead of node editor class
			//NodeEditorWindow.current.Repaint();
		}
		GUILayout.EndVertical();

		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("prompt"), GUIContent.none);
		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Time", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Time")).x));
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("time"), GUIContent.none);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Speaker Image", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Speaker Image:")).x));
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("speakerPic"), GUIContent.none);
		GUILayout.EndHorizontal();

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