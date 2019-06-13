using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(BaseNode))]
public class BaseNodeEditor : NodeEditor
{
	public bool openFoldout;
	[SerializeField] public UnityEngine.UI.Image pic;

	public override void OnBodyGUI()
	{
		serializedObject.Update();

		BaseNode node = target as BaseNode;
		SerializedObject so = new SerializedObject(target);
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
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("time"), new GUIContent("Time"));

		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("speakerPic"), new GUIContent("Speaker Image"));
		//NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("action"), GUIContent.none);
		//pic = (UnityEngine.UI.Image) EditorGUILayout.ObjectField("pic", pic, typeof (UnityEngine.UI.Image), true);

		NodeEditorGUILayout.InstancePortList("answers", typeof(BaseNode), serializedObject, XNode.NodePort.IO.Output, XNode.Node.ConnectionType.Override);

		serializedObject.ApplyModifiedProperties();
	}

	public override int GetWidth()
	{
		return 300;
	}
}