using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(Label))]
public class LabelNodeEditor : NodeEditor
{
	public override void OnBodyGUI()
	{
		serializedObject.Update();

		Label node = target as Label;

		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("label"), GUIContent.none);

		serializedObject.ApplyModifiedProperties();
	}

}