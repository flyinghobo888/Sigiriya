#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TransformSaver))]
public class TransformSaverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TransformSaver myScript = (TransformSaver)target;

        if (GUILayout.Button("Save Transform"))
        {
            myScript.SaveTransform();
        }
    }
}
#endif