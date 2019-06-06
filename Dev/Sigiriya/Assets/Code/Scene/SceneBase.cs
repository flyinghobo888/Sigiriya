using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBase<T> : ManagerBase<T> where T : ManagerBase<T>
{
    private static GameObject persistenceInstance;
    public bool sceneFade = true;

    protected void Awake()
    {
        if (!persistenceInstance)
        {
            GameObject persistenceRes = Resources.Load<GameObject>("Unity/Prefabs/_PERSISTENT");
            persistenceInstance = Instantiate(persistenceRes);
            persistenceInstance.name = "_PERSISTENT";
        }

        if (sceneFade)
        {
            SceneNavigator.Instance.FadeOutToScene();
        }
    }
}
