//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTest : MonoBehaviour
{
    private int sceneIndex;

    private void Start()
    {
        sceneIndex = (int)SceneNavigator.Instance.CurrentScene;
    }

	private void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    EventAnnouncer.OnRequestSceneChange((EnumScene)((++sceneIndex) % (int)EnumScene.SIZE), true);
        //}
    }
}