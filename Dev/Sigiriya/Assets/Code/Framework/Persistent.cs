using UnityEngine;

//This script is not meant to have other things in it.
//It's meant to be put on one object to make that object persistent across scenes.
//You can make another script and put it on the same gameobject, but please keep this script as is.
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
