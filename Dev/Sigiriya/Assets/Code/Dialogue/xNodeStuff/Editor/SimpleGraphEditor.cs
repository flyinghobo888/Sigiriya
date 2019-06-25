using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using XNode;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(SimpleGraph))]
public class SimpleGraphEditor : NodeGraphEditor
{
	public override void OnGUI()
	{
		// Keep repainting the GUI of the active NodeEditorWindow
		NodeEditorWindow.current.Repaint();
	}
}