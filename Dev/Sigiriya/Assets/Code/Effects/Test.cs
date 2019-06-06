using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private int sceneIndex;

    private void Start()
    {
        sceneIndex = (int)SceneNavigator.Instance.CurrentScene;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventAnnouncer.OnRequestSceneChange((EnumScene)((++sceneIndex) % (int)EnumScene.SIZE), true);
        }
    }
}
