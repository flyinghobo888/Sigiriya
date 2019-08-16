using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SceneBase<GameScene>
{
	private void OnEnable()
    {
        Debug.Log("Game scene enabled!");
        DialogueController.Instance.RestartAllDialogue();
    }
}
