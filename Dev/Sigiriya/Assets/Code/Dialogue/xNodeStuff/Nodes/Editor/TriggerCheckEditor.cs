﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;
using UnityEditor;

[CustomNodeEditor(typeof(TriggerCheck))]
public class StringNodeEditor : NodeEditor
{
	//FlagBank.Flags m_flag;
	TriggerCheck testNode;

	public override void OnBodyGUI()
	{
		serializedObject.Update();

		TriggerCheck node = target as TriggerCheck;

		testNode = node;

		if (EditorGUILayout.DropdownButton(new GUIContent(node.trigger.ToString()), FocusType.Passive))
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

		node.trigger = testNode.trigger;

		GUILayout.BeginHorizontal();
		NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("operatorType"), GUIContent.none);
		NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("result"), GUILayout.MinWidth(0));
		GUILayout.EndHorizontal();

		serializedObject.ApplyModifiedProperties();
	}

	void AddMenuItemForFlag(GenericMenu menu, string menuPath, FlagBank.Flags flag)
	{
		// the menu item is marked as selected if it matches the current value of m_Color
		menu.AddItem(new GUIContent(menuPath), testNode.trigger.Equals(flag), OnFlagSelected, flag);
	}

	// the GenericMenu.MenuFunction2 event handler for when a menu item is selected
	void OnFlagSelected(object mFlag)
	{
		testNode.trigger = (FlagBank.Flags)mFlag;
	}

	void CreateSubmenu(GenericMenu menu, int index)
	{
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
			nextSubmenu = FlagBank.flagList.Count - 1;
		}

		//get each flag from the submenu to the next submenu
		for (; currSubmenu < nextSubmenu; currSubmenu++)
		{
			FlagBank.Flags tmpFlag = FlagBank.flagList[currSubmenu + 1];
			AddMenuItemForFlag(menu, submenuTrigger + "/" + tmpFlag, tmpFlag);
		}

		//if we aren't at the end, add a separator
		if (index + 1 < FlagBank.submenuList.Count)
		{
			menu.AddSeparator("");
		}
	}

	public override int GetWidth()
	{
		return 150;
	}
}
