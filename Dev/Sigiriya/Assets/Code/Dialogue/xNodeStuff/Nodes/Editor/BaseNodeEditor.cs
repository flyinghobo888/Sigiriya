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

		serializedObject.ApplyModifiedProperties();
	}

	public override int GetWidth()
	{
		return 300;
	}
}