using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(Probability))]
public class ProbabilityNodeEditor : NodeEditor
{
	public override void OnBodyGUI()
	{
		serializedObject.Update();

		Probability node = target as Probability;

		node.UpdateModLength();
		NodeEditorGUILayout.InstancePortList("args", typeof(bool), serializedObject, XNode.NodePort.IO.Input, XNode.Node.ConnectionType.Multiple, XNode.Node.TypeConstraint.Strict);

		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("baseProbability"));
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("modVal"));
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("result"));

		serializedObject.ApplyModifiedProperties();
	}
}
