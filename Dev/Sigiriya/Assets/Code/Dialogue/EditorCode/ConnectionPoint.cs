using System;
using UnityEditor;
using UnityEngine;

public enum ConnectionPointType { IN, OUT }

public class ConnectionPoint
{
	public Rect rect;
	public ConnectionPointType type;
	public Node node;
	public GUIStyle style;
	public Action<ConnectionPoint> OnClickConnectionPoint;
	private bool isResponsePoint = false;

	public ConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint)
	{
		this.node = node;
		this.type = type;
		this.style = style;
		this.OnClickConnectionPoint = OnClickConnectionPoint;
		this.rect = new Rect(0, 0, 10f, 20f);
	}
	public ConnectionPoint(Node node, Rect rect, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint)
	{
		this.node = node;
		this.type = type;
		this.style = style;
		this.OnClickConnectionPoint = OnClickConnectionPoint;
		this.rect = new Rect(rect.x, rect.y, 10f, 20f);
		isResponsePoint = true;
	}

	public void Draw()
	{
		if (!isResponsePoint)
		{
			rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;
		}

		switch (type)
		{
			case ConnectionPointType.IN:
				rect.x = node.rect.x - rect.width + 8f;
				break;

			case ConnectionPointType.OUT:
				rect.x = node.rect.x + node.rect.width - 8f;
				break;
		}

		if (GUI.Button(rect, "", style))
		{
			if (OnClickConnectionPoint != null)
			{
				OnClickConnectionPoint(this);
			}
		}
	}
}
