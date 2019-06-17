using System.Collections;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Connection
{
	public ConnectionPoint inPoint;
	public ConnectionPoint outPoint;
	public Action<Connection> OnClickRemoveConnection;

	public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
	{
		this.inPoint = inPoint;
		this.outPoint = outPoint;
		this.OnClickRemoveConnection = OnClickRemoveConnection;
	}

	public void Draw()
	{
		Handles.DrawBezier(
		inPoint.rect.center,
		outPoint.rect.center,
		inPoint.rect.center + Vector2.left * 50f,
		outPoint.rect.center - Vector2.left * 50,
		Color.white,
		null,
		2f
		);

		if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
		{
			if (OnClickRemoveConnection != null)
			{
				OnClickRemoveConnection(this);
			}
		}
	}

	//HACK: don't need this?
	public void InvokeRemoveConnection()
	{
		OnClickRemoveConnection(this);
	}
}
