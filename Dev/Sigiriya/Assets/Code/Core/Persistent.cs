using UnityEngine;

//Authors: Andrew Rimpici
//This script is not meant to have other things in it.
//It's meant to be put on an object to make that object persistent across scenes.
//You can make another script and put it on that gameobject, but please keep this script as is.
public class Persistent : MonoBehaviour
{
    private static GameObject instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
