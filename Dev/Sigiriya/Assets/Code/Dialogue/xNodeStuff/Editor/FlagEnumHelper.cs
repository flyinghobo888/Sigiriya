using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FlagEnumHelper
{
	public bool canCopy = false;
	public FlagBank.Flags m_flag = FlagBank.Flags.NONE;

	public void InitFlagHelper(bool copy, FlagBank.Flags flag)
	{
		canCopy = copy;
		 m_flag = flag;
		//Debug.Log(flag);
	}
	public void CopyFlagHelper(out bool copy, out FlagBank.Flags flag)
	{
		copy = canCopy;
		flag = m_flag;
		//Debug.Log(flag);
	}

	public void CreateMenu(FlagBank.Flags flag, out FlagBank.Flags throwFlag)
	{
		if (canCopy)
		{
			m_flag = flag;
			canCopy = false;
		}

		if (EditorGUILayout.DropdownButton(new GUIContent(flag.ToString()), FocusType.Passive))
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
		throwFlag = m_flag;
	}

	public void AddMenuItemForFlag(GenericMenu menu, string menuPath, FlagBank.Flags flag)
	{
		// the menu item is marked as selected if it matches the current value of m_Color
		menu.AddItem(new GUIContent(menuPath), m_flag.Equals(flag), OnFlagSelected, flag);
	}

	// the GenericMenu.MenuFunction2 event handler for when a menu item is selected
	public void OnFlagSelected(object mFlag)
	{
		m_flag = (FlagBank.Flags)mFlag;
	}

	public void CreateSubmenu(GenericMenu menu, int index)
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
}
