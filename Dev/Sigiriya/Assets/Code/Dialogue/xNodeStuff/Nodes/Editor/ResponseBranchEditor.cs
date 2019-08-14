using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(ResponseBranchNode))]
public class ResponseBranchNodeEditor : NodeEditor
{

	public override void OnBodyGUI()
	{
		serializedObject.Update();

		ResponseBranchNode node = target as ResponseBranchNode;

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
		node.CheckConnectedColor();
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("connected"), GUIContent.none);
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();

		NodeEditorGUILayout.InstancePortList("responses", typeof(ResponseNode), serializedObject, XNode.NodePort.IO.Output, XNode.Node.ConnectionType.Override);

		serializedObject.ApplyModifiedProperties();
	}

}
