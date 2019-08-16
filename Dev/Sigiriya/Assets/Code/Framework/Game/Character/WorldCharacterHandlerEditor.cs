#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldCharacterHandler))]
public class WorldCharacterHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WorldCharacterHandler myScript = (WorldCharacterHandler)target;

        if (GUILayout.Button("Update"))
        {
            myScript.SetCharacter(myScript.ResetConvo);
        }
    }
}
#endif