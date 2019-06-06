using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Authors: Andrew Rimpici
//All managers that there should only be one of at a time in a scene can extend this class and then it will make sure there are no duplicates.
public class ManagerBase<T> : MonoBehaviour where T : MonoBehaviour
{
    //The current manager instance is stored in this instance variable.
    private static T instance;

    public static T Instance
    {
        get
        {
            T objOfType = FindObjectOfType<T>();
            
            if (!instance)
            {
                instance = objOfType;
            }
            else if (instance != objOfType)
            {
                Destroy(objOfType);
            }

            return instance;
        }
    }
}
