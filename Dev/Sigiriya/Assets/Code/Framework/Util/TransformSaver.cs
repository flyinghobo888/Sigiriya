using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TransformSaver : MonoBehaviour
{
    private const string CHAR_PATH = "Assets/Code/Data/Transform/Character/";

    [Header("Character Scriptable Obj to Bind")]
    public Character characterToBind = null;

    [Header("Character Transform")]
    public RectTransform characterTransform = null;

    [Header("Character Mood (Leave none if not applicable)")]
    public EnumMood characterMood = EnumMood.NONE;

    [Header("Object Transforms - Make sure they have tags")]
    public List<RectTransform> transformsToSave = new List<RectTransform>();

    [Header("Scriptable Obj Save Name")]
    public string transformName = "";

    public void SaveTransform()
    {
        if (characterTransform && transformsToSave.Count > 0 && characterToBind && transformName != "")
        {
            Directory.CreateDirectory(CHAR_PATH + transformName);
            Directory.CreateDirectory(CHAR_PATH + transformName + "/Object");

            CharacterTransform charTransform = ScriptableObject.CreateInstance<CharacterTransform>();
            charTransform.Init(characterTransform);
            charTransform.AddMood(characterMood);

            foreach (RectTransform t in transformsToSave)
            {
                if (t)
                {
                    ObjectTransform objTransform = ScriptableObject.CreateInstance<ObjectTransform>();
                    objTransform.Init(t);

                    //Create the object transform scriptable object
                    AssetDatabase.CreateAsset(objTransform, CHAR_PATH + transformName + "/Object/" + transformName + "_" + t.tag + ".asset");

                    charTransform.AddObject(objTransform);
                }
            }

            //Create the character transform scriptable object
            AssetDatabase.CreateAsset(charTransform, CHAR_PATH + transformName + "/" + transformName + ".asset");

            //Bind the character transform scriptable object to the character scriptable object
            characterToBind.characterTransforms.Add(charTransform);

            transformsToSave = null;
            characterTransform = null;
            characterToBind = null;
            transformName = "";
        }
        else
        {
            Debug.LogWarning("Please insert your transform, character, and transform name!");
        }
    }
}
