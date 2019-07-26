using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : SceneBase<GameScene>
{
	[SerializeField] private DialogueController sceneDialogueController;

	private void OnEnable()
    {
        Debug.Log("Game scene enabled!");

        //This is just to initialize the dialoguecontroller instance
        sceneDialogueController.gameObject.SetActive(true);
        DialogueController.Instance.RestartAllDialogue();
        DialogueController.Instance.gameObject.SetActive(false);
    }
}
