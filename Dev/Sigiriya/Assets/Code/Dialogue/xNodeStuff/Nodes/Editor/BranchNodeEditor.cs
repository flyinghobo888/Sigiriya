using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(BranchNode))]
public class BranchNodeEditor : NodeEditor
{
	public override void OnBodyGUI()
	{
		serializedObject.Update();

		BranchNode node = target as BranchNode;

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();

		NodeEditorGUILayout.InstancePortList("branches", typeof(BranchCheck), serializedObject, XNode.NodePort.IO.Output, XNode.Node.ConnectionType.Override);

		serializedObject.ApplyModifiedProperties();
	}

	public override int GetWidth()
	{
		return 150;
	}

}
