using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldDepthFromHeight))]
public class WorldDepthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WorldDepthFromHeight myScript = (WorldDepthFromHeight)target;

        if (GUILayout.Button("Reorder"))
        {
            myScript.UpdateDepth();
        }
    }
}
