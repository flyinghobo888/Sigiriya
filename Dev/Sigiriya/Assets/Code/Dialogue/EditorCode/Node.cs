using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Node
{
	/*
	 Node Notes:

		-The overall box should have drop down tabs that can resize the entire node box thing
			-so I gotta figure out how to resize things
		 */

	public Rect rect;
	private Rect promptRect;

	private Vector2 rectBuffer = new Vector2(50, 10);

	public string title;
	public bool isDragged;
	public bool isSelected;
	public bool openFoldout;

	public ConnectionPoint inPoint;
	public ConnectionPoint outPoint;

	public GUIStyle style;
	public GUIStyle defaultNodeStyle;
	public GUIStyle selectedNodeStyle;

	public Action<Node> OnRemoveNode;

	int numResponses = 0;

	string promptString = "";
	List<ConnectionPoint> responsePoints; //Make list?

	public Node(DialogueController.DialogueNode dialogueNode, Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint, Action<Node> OnClickRemoveNode)
	{
		//TODO: pass values for promptRect in maybe? Or not? 
		//Also, this is fuckin gross, fix the sizing thing please
		promptRect = new Rect(position.x + rectBuffer.x / 2, position.y + rectBuffer.y, width - rectBuffer.x, (height / 2));
		responsePoints = new List<ConnectionPoint>();

		rect = new Rect(position.x, position.y, width, height);
		style = nodeStyle;
		inPoint = new ConnectionPoint(this, ConnectionPointType.IN, outPointStyle, onClickInPoint);
		outPoint = new ConnectionPoint(this, ConnectionPointType.OUT, outPointStyle, onClickOutPoint);
		defaultNodeStyle = nodeStyle;
		selectedNodeStyle = selectedStyle;
		OnRemoveNode = OnClickRemoveNode;

		//TODO: if dialogueNode is null, add a node to the controller
		if (dialogueNode != null)
		{
			promptString = dialogueNode.prompt;
		}
	}

	public void Drag(Vector2 delta)
	{
		rect.position += delta;
		promptRect.position += delta;
	}

	public void Draw()
	{
		inPoint.Draw();
		outPoint.Draw();

		for (int i = 0; i < numResponses; i++)
		{
			responsePoints[i].Draw();
		}

		GUILayout.BeginArea(rect, title, style);
		//GUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Prompt:", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Prompt:")).x));
		promptString = GUILayout.TextArea(promptString);
		//GUILayout.EndHorizontal();

		GUILayout.BeginVertical("Responses:");
		openFoldout = EditorGUILayout.Foldout(openFoldout, "Responses:", EditorStyles.boldFont);
		if (openFoldout)
		{
			//vertical layout group with a horizontal for each response

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Number of Responses:", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Number of Responses:")).x));
			if (GUILayout.Button("Add Response"))
			{
				numResponses++;
				responsePoints.Add(new ConnectionPoint(this, rect, ConnectionPointType.OUT, outPoint.style, outPoint.OnClickConnectionPoint));
			}
			GUILayout.EndHorizontal();

			for (int i = 0; i < numResponses; i++)
			{
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Response " + i + ":", GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent("Response 1:")).x));
				GUILayout.TextField("Not Implemented Yet");
				if (Event.current.type == EventType.Repaint)
				{
					Rect lastRect = GUILayoutUtility.GetLastRect();
					responsePoints[i].rect = new Rect(responsePoints[i].rect) {y = lastRect.y + rect.y };
				}
				if (GUILayout.Button("Delete Response"))
				{
					//TODO: Connection line still draws after this is deleted. Fix pls
					responsePoints[i] = null;
					responsePoints.Remove(responsePoints[i]);
					numResponses--;
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndVertical();

		GUILayout.EndArea();
	}

	public bool ProcessEvents(Event e)
	{
		switch (e.type)
		{
			case EventType.MouseDown:
				if (e.button == 0)
				{
					if (rect.Contains(e.mousePosition))
					{
						isDragged = true;
						GUI.changed = true;
						isSelected = true;
						style = selectedNodeStyle;
					}
					else
					{
						GUI.changed = true;
						isSelected = false;
						style = defaultNodeStyle;
					}
				}

				if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
				{
					ProcessContextMenu();
					e.Use();
				}
				break;

			case EventType.MouseUp:
				isDragged = false;
				break;

			case EventType.MouseDrag:
				if (e.button == 0 && isDragged)
				{
					Drag(e.delta);
					e.Use();
					return true;
				}
				break;
		}

		return false;
	}

	private void ProcessContextMenu()
	{
		GenericMenu genericMenu = new GenericMenu();
		genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
		genericMenu.ShowAsContext();
	}

	private void OnClickRemoveNode()
	{
		if (OnRemoveNode != null)
		{
			OnRemoveNode(this);
		}
	}
}
