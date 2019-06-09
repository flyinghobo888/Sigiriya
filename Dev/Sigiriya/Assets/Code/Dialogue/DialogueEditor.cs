﻿/*
 Underlying node code base found at http://gram.gs/gramlog/creating-node-based-editor-unity/ 
  */


using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueEditor : EditorWindow
{
	string myString = "string";
	bool groupEnabled = false;
	bool myBool = false;
	float myFloat = 0.0f;
	DialogueController currObjectDialogue;

	private List<Node> nodes;
	private List<Connection> connections;

	private GUIStyle nodeStyle;
	private GUIStyle selectedNodeStyle;
	private GUIStyle inPointStyle;
	private GUIStyle outPointStyle;

	private ConnectionPoint selectedInPoint;
	private ConnectionPoint selectedOutPoint;

	private Vector2 offset;
	private Vector2 drag; //Canvas drag

	[MenuItem("Dialogue System/DialogueEditor")]
	public static void ShowWindow()
	{
		GetWindow(typeof(DialogueEditor));
	}

	#region GUI Unity Functions
	private void OnEnable()
	{
		nodeStyle = new GUIStyle();
		nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
		nodeStyle.border = new RectOffset(12, 12, 12, 12);

		selectedNodeStyle = new GUIStyle();
		selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
		selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

		inPointStyle = new GUIStyle();
		inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
		inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
		inPointStyle.border = new RectOffset(4, 4, 12, 12);

		outPointStyle = new GUIStyle();
		outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
		outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
		outPointStyle.border = new RectOffset(4, 4, 12, 12);

		//TODO: Run Selection.ischanged or whatever it is to check if selection is changed
		if (Selection.activeGameObject == null)
		{
			Debug.Log("Select an Object!");
		}
		else
		{
			//TODO: Also check if that component exists. This might/will crash all of Unity if error handling doesn't exist
			currObjectDialogue = Selection.activeGameObject.GetComponent<DialogueController>();
			//TODO: We have access to the dialogue, use loop to create many nodes

			if (currObjectDialogue.nodes.Count > 0)
			{
				if (nodes == null)
				{
					nodes = new List<Node>();
				}

				Debug.Log("SETUP EXISTING");

				//TODO: Remove those magic numbers for node size
				nodes.Add(new Node(currObjectDialogue.nodes[0], Vector2.zero, 600, 600, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
			}
		}

	}
	void OnGUI()
	{


		DrawGrid(20, 0.2f, Color.gray);
		DrawGrid(100, 0.4f, Color.gray);

		DrawNodes();
		DrawConnections();

		DrawConnectionLine(Event.current);

		ProcessNodeEvents(Event.current);
		ProcessEvents(Event.current);
	
		if (GUI.changed) Repaint();
	}
	#endregion

	#region Private Functions
	private void DrawNodes()
	{
		if (nodes!= null)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				nodes[i].Draw();
			}
		}
	}

	private void DrawConnections()
	{
		if (connections != null)
		{
			for (int i = 0; i < connections.Count; i++)
			{
				connections[i].Draw();
			}
		}
	}

	private void DrawConnectionLine(Event e)
	{
		if (selectedInPoint != null && selectedOutPoint == null)
		{
			Handles.DrawBezier(
				selectedInPoint.rect.center,
				e.mousePosition,
				selectedInPoint.rect.center + Vector2.left * 50f,
				e.mousePosition - Vector2.left * 50f,
				Color.white,
				null,
				2f
				);

			GUI.changed = true;
		}

		if (selectedOutPoint != null && selectedInPoint == null)
		{
			Handles.DrawBezier(
				selectedOutPoint.rect.center,
				e.mousePosition,
				selectedOutPoint.rect.center - Vector2.left * 50f,
				e.mousePosition + Vector2.left * 50f,
				Color.white,
				null,
				2f
				);

			GUI.changed = true;
		}
	}

	private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
	{
		int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
		int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

		Handles.BeginGUI();
		Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

		offset += drag * 0.5f;
		Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

		for (int i = 0; i < widthDivs; i++)
		{
			Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
		}

		for (int j = 0; j < widthDivs; j++)
		{
			Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
		}

		Handles.color = Color.white;
		Handles.EndGUI();
	}

	private void ProcessEvents(Event e)
	{
		drag = Vector2.zero;

		switch (e.type)
		{
			case EventType.MouseDown:
				if (e.button == 1)
				{					
					if (!StopDrawingMouseCurve())
					{
						ProcessContextMenu(e.mousePosition);
					}
				
				}
			break;

			case EventType.MouseDrag:
				if(e.button == 0)
				{
					OnDrag(e.delta);
				}
			break;
		}
	}

	private void ProcessNodeEvents(Event e)
	{
		if (nodes != null)
		{
			for (int i = nodes.Count - 1; i >= 0; i--)
			{
				bool guiChanged = nodes[i].ProcessEvents(e);

				if (guiChanged)
				{
					GUI.changed = true;
				}
			}
		}
	}

	private void ProcessContextMenu(Vector2 mousePosition)
	{
		GenericMenu genericMenu = new GenericMenu();
		genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
		genericMenu.ShowAsContext();
	}

	private void OnClickAddNode(Vector2 mousePosition)
	{
		if (nodes == null)
		{
			nodes = new List<Node>();
		}

		//TODO: Remove those magic numbers for node size
		nodes.Add(new Node(null, mousePosition, 600, 600, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
	}

	private void OnClickInPoint(ConnectionPoint inPoint)
	{
		selectedInPoint = inPoint;

		if (selectedOutPoint != null)
		{
			if (selectedOutPoint.node != selectedInPoint.node)
			{
				CreateConnection();
				ClearConnectionSelection();
			}
			else
			{
				ClearConnectionSelection();
			}
		}
	}

	private void OnClickOutPoint(ConnectionPoint outPoint)
	{
		selectedOutPoint = outPoint;

		if (selectedInPoint != null)
		{
			if (selectedOutPoint.node != selectedInPoint.node)
			{
				CreateConnection();
				ClearConnectionSelection();
			}
			else
			{
				ClearConnectionSelection();
			}
		}
	}

	private bool StopDrawingMouseCurve()
	{
		if (selectedInPoint != null || selectedOutPoint != null)
		{
			ClearConnectionSelection();
			return true;
		}

		return false;
	}

	private void OnClickRemoveConnection(Connection connection)
	{
		connections.Remove(connection);
	}

	private void OnClickRemoveNode(Node node)
	{
		if (connections != null)
		{
			List<Connection> connectionsToRemove = new List<Connection>();

			for (int i = 0; i < connections.Count; i++)
			{
				if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
				{
					connectionsToRemove.Add(connections[i]);
				}
			}

			//why is this loop here? couldn't it be done above?
			for (int i = 0; i < connectionsToRemove.Count; i++)
			{
				connections.Remove(connectionsToRemove[i]);
			}

			connectionsToRemove = null;
		}

		nodes.Remove(node);
	}

	private void OnDrag(Vector2 delta)
	{
		drag = delta;

		if (nodes != null)
		{
			for (int i = 0; i < nodes.Count; i++)
			{
				nodes[i].Drag(delta);
			}

			GUI.changed = true;
		}
	}

	private void CreateConnection()
	{
		if (connections == null)
		{
			connections = new List<Connection>();
		}

		connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
	}

	private void ClearConnectionSelection()
	{
		selectedInPoint = null;
		selectedOutPoint = null;
	}

	#endregion

	#region Testing Stuff
	void TestGUIStuff()
	{
		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		//get the object of type(whateverYouWant) and specify if allowSceneObjects is true/false
		currObjectDialogue = EditorGUILayout.ObjectField((UnityEngine.Object)currObjectDialogue, typeof(DialogueController), true) as DialogueController;

		myString = EditorGUILayout.TextField("Text Field", myString);
		currObjectDialogue.SetStringVal(myString);//


		groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle("Toggle", myBool);
		myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
		EditorGUILayout.EndToggleGroup();

	}
	#endregion
}
