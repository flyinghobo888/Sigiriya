using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
	[SerializeField] private GameObject characterContainer;
	[SerializeField] private GameObject talkBubble;
	[SerializeField] private SimpleGraph dialogueGraph;
    //[SerializeField] private DialogueController dCon;

    //TODO: instead of this script, rename it to ButtonInfo or something...
    //and have the button call functions from here 
    //Also, all graphs need to be initialized so that the bubbles work properly
    //as it stands, they are only init when you click on the button.
    //this needs to happen on startup, when graphs swap, etc.

    private void Awake()
    {
        CheckIfWantsToTalk();
    }

    private void OnEnable()
	{
        EventAnnouncer.OnDialogueRestart += CheckIfWantsToTalk;

        CheckIfWantsToTalk();
    }

	private void OnDisable()
	{
        EventAnnouncer.OnDialogueRestart -= CheckIfWantsToTalk;

        CheckIfWantsToTalk();
    }

	void CheckIfWantsToTalk()
	{
		if (DialogueController.Instance != null && talkBubble != null)
		{
			if (!dialogueGraph.isInit)
			{
				dialogueGraph.Restart();
			}

			if (dialogueGraph.current == null)
			{
				talkBubble.SetActive(false);
			}
			else
			{
				talkBubble.SetActive(true);
            }
		}
	}

	public void PressButton()
	{
		EnableCurrNode();
		DisableCharacter();
	}

	public void EnableCurrNode()
	{
		DialogueController.Instance.EnableCurrNode(dialogueGraph);
	}

	public void DisableCharacter()
	{
		DialogueController.Instance.DisableCharacterController(characterContainer);
	}
}
