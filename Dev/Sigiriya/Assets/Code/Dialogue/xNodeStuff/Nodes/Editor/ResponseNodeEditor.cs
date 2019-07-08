using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(ResponseNode))]
public class ResponseNodeEditor : NodeEditor
{
	FlagBank.Flags m_flag;
	bool canCopy = true;

	public override void OnBodyGUI()
	{
		serializedObject.Update();

		ResponseNode node = target as ResponseNode;
		SerializedObject so = new SerializedObject(target);

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("promptInput"), GUILayout.MinWidth(0));
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();

		GUILayout.BeginVertical();
		NodeEditorGUILayout.PortField(new GUIContent("Hidden Status"), target.GetInputPort("isHidden"), GUILayout.MinWidth(0));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Quip");
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("textButton"), GUIContent.none);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Full ");
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("textFull"), GUIContent.none);
		GUILayout.EndHorizontal();

		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("voiceClip"), GUIContent.none);
		CreateMenu(node);
		GUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();
	}

	void CreateMenu(ResponseNode node)
	{
		if (canCopy)
		{
			m_flag = node.throwFlag;
			canCopy = false;
		}

		if (EditorGUILayout.DropdownButton(new GUIContent(node.throwFlag.ToString()), FocusType.Passive))
		{
			GenericMenu menu = new GenericMenu();

			//for each submenu, make one
			for (int i = 0; i < FlagBank.submenuList.Count; i++)
			{
				CreateSubmenu(menu, i);
			}
			// display the menu
			menu.ShowAsContext();
		}
		node.throwFlag = m_flag;
	}

	void AddMenuItemForFlag(GenericMenu menu, string menuPath, FlagBank.Flags flag)
	{
		// the menu item is marked as selected if it matches the current value of m_Color
		menu.AddItem(new GUIContent(menuPath), m_flag.Equals(flag), OnFlagSelected, flag);
	}

	// the GenericMenu.MenuFunction2 event handler for when a menu item is selected
	void OnFlagSelected(object mFlag)
	{
		m_flag = (FlagBank.Flags)mFlag;
	}

	void CreateSubmenu(GenericMenu menu, int index)
	{
		FlagBank.Flags[] flagList = ((FlagBank.Flags[])System.Enum.GetValues(typeof(FlagBank.Flags)));

		int currSubmenu = (int)FlagBank.submenuList[index];
		string submenuTrigger = FlagBank.submenuList[index].ToString();
		int nextSubmenu;

		//set the next submenu, or the end of the list as the next submenu
		if (index + 1 < FlagBank.submenuList.Count)
		{
			nextSubmenu = (int)FlagBank.submenuList[index + 1] - 1;
		}
		else
		{
			nextSubmenu = flagList.Length - 1;
		}

		//get each flag from the submenu to the next submenu
		for (; currSubmenu < nextSubmenu; currSubmenu++)
		{
			FlagBank.Flags tmpFlag = flagList[currSubmenu + 1];
			AddMenuItemForFlag(menu, submenuTrigger + "/" + tmpFlag, tmpFlag);
		}

		//if we aren't at the end, add a separator
		if (index + 1 < FlagBank.submenuList.Count)
		{
			menu.AddSeparator("");
		}
		else
		{
			menu.AddSeparator("");
			AddMenuItemForFlag(menu, FlagBank.Flags.NONE.ToString(), FlagBank.Flags.NONE);
		}
	}


	public override int GetWidth()
	{
		return 200;
	}
}
