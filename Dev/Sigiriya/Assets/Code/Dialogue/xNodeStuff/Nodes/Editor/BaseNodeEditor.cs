using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(BaseNode))]
public class BaseNodeEditor : NodeEditor
{
	public bool openFoldout;
	Sprite m_sprite;
	[SerializeField] public UnityEngine.UI.Image pic;


	public override void OnBodyGUI()
	{
		serializedObject.Update();

		BaseNode node = target as BaseNode;
		SerializedObject so = new SerializedObject(target);

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();



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
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("usedSprite"), new GUIContent("Speaker Image"));

		if (EditorGUILayout.DropdownButton(new GUIContent("Test"), FocusType.Passive))
		{
			GenericMenu menu = new GenericMenu();

			//AddMenuItemForSprite(menu, "",); ///Don't know how to get the value from a list
		}
		//NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("action"), GUIContent.none);
		//pic = (UnityEngine.UI.Image) EditorGUILayout.ObjectField("pic", pic, typeof (UnityEngine.UI.Image), true);

		NodeEditorGUILayout.InstancePortList("answers", typeof(BaseNode), serializedObject, XNode.NodePort.IO.Output, XNode.Node.ConnectionType.Override);

		serializedObject.ApplyModifiedProperties();
	}

	void AddMenuItemForSprite(GenericMenu menu, string menuPath, List<Sprite> sprite)
	{
		// the menu item is marked as selected if it matches the current value of m_Color
		menu.AddItem(new GUIContent(menuPath), m_sprite.Equals(sprite), OnSpriteSelected, sprite);
	}

	// the GenericMenu.MenuFunction2 event handler for when a menu item is selected
	void OnSpriteSelected(object sprite)
	{
		m_sprite = (Sprite)sprite;
	}

	public override int GetWidth()
	{
		return 300;
	}
}