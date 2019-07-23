using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

/*
TODO:
-move text scaling
-make dimensions CONSTANT

*/

public class DialogueController : ManagerBase<DialogueController>
{
	//[Header("Camera")]
	//[SerializeField] private int textSize;

	//Graph where nodes are kept
	public SimpleGraph dialogueGraph;

	[Header("UI Fields")]
	[SerializeField] private TextMeshProUGUI nameBox = null;
	[SerializeField] private TextMeshProUGUI promptPanel = null;
	[SerializeField] private Button[] responseButtons = null;
	[SerializeField] private Button continueButton = null;
	[SerializeField] private List<Image> speakerImages = null; //basically just a holder of images to display

    [SerializeField] private GameObject characterContainer = null;

	[Header("Dialogue")]
	//[SerializeField] PromptNode pNode;
	private string ID;
	private BaseNode checkPointNode = null;
	private BaseNode exitNode = null;

	//private float talkTimer = 0;

	[Header("Dev/Editor")]
	[SerializeField] private List<SimpleGraph> graphList = null;

	//private void Awake()
	//{
	//	if (dialogueGraph != null)
	//	{
	//		if (dialogueGraph.isInit)
	//		{
	//			Instance.Init();
	//			Instance.gameObject.SetActive(false);
	//		}
	//	}
	//	else
	//	{
	//		Instance.gameObject.SetActive(false);
	//	}
	//}

	private void Init()
	{
		ID = this.name;

		dialogueGraph.Restart();

		AssessCurrentType();
	}

	private void OnEnable()
	{
		//EventManager.StartListening("E_down", ContinueDialogue);
		//EventManager.FireEvent("MENU_open");

		//TODO: Save data somewhere, somehow, cause the dialogueGraph.current isn't actually saved on exit :(

		PersistentEventBank.FireAllEvents();
	}
	private void OnDisable()
	{
		//EventManager.StopListening("E_down", ContinueDialogue);
		//EventManager.FireEvent("MENU_close");

		EventAnnouncer.OnDialogueEnd?.Invoke();
	}

	private void Start()
	{
		//Maybe set extraConnections again here? I dunno.

		//move to start once text size is decided
		//textSize = textSize * Screen.width / 1920;

	}

	private void Update()
	{
        //PromptNode pNode = dialogueGraph.current as PromptNode;

        //null should be a signal to end the current discussion
        //if (dialogueGraph.current != null && enabled)
        //{
        //	//this can probs be moved out of update tbh
        //	//DisplayNode(dialogueGraph.current);
        //}
        //else
        //{
        //	Debug.Log(ID + " is done talking");
        //	gameObject.SetActive(false);
        //          characterContainer.SetActive(true);

        //          dialogueGraph.current = exitNode;

        //          //conversation is done.
        //}

        //int tempDefaultTime = 3;
        ////TODO: the time variable should be moved from promptNode to baseNode
        //if (tempDefaultTime != 0)//pNode.time != 0)
        //{
        //    talkTimer += Time.deltaTime;
        //}
        //if (talkTimer > tempDefaultTime)
        //{
        //    ContinueDialogue();

        //    talkTimer = 0;
        //}
    }

	//Display node can only display PromptNodes. This might need to also support ResponseNodes
	void DisplayNode(BaseNode node)
	{
		if (node.GetType() != null && node.GetType() == typeof(PromptNode))
		{
			PromptNode pNode = node as PromptNode;

			checkPointNode = pNode.GetConnectedNode("checkpointConnection");
			exitNode = pNode.GetConnectedNode("exitConnection");

			nameBox.text = pNode.speaker == null ? "Player" : pNode.speaker.characterName;
            promptPanel.text = "";//pNode.prompt;
            EventAnnouncer.OnDialogueUpdate(promptPanel, pNode.prompt);

			int i = 0;

			if (pNode.responses.Count != 0) //if we have responses
			{
                for (; i < pNode.responses.Count; i++)
                {
                    if (!pNode.GetAnswerConnection(i).getHidden())
                    {
                        responseButtons[i].gameObject.SetActive(true);
                        //TODO: update the response class with two(2) text things. a button text, and full text
                        responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = pNode.GetAnswerConnection(i).textButton;
                    }
                    else
                    {
                        responseButtons[i].gameObject.SetActive(false);
                    }
                }

				continueButton.gameObject.SetActive(false);
			}
			else
			{
				//TODO: Timed dialogue continues now, so remove this gracefully after the continue button becomes obsolete
				continueButton.gameObject.SetActive(true);
			}

			for (; i < responseButtons.Length; i++)
			{
				responseButtons[i].gameObject.SetActive(false);
			}
		}
		else if(node.GetType() == typeof(ResponseNode))
		{
			ResponseNode rNode = node as ResponseNode;

            nameBox.text = "Player";
            if (rNode.textFull != "")
			{
                promptPanel.text = "";//rNode.textFull;
                EventAnnouncer.OnDialogueUpdate(promptPanel, rNode.textFull);
            }
			else
			{
                promptPanel.text = "";//rNode.textButton;
                EventAnnouncer.OnDialogueUpdate(promptPanel, rNode.textButton);
            }

			Debug.Log("PLAYER TALKING");
			int i = 0;

			for (; i < responseButtons.Length; i++)
			{
				responseButtons[i].gameObject.SetActive(false);
			}

            continueButton.gameObject.SetActive(true);
        }
	}

	#region Continuation Commands
	//goes to the next default connected node
	public void ContinueDialogue()
	{
        if (!TextController.Instance.IsFinished)
        {
            EventAnnouncer.OnDialogueRequestFinish(promptPanel);
            return;
        }

		//Activate the listening image while I still have the emotion
		SetSpeakerImage(false);

		dialogueGraph.current = dialogueGraph.current.GetNextNode();
		AssessCurrentType();

		//activate the talking for the speaker
		SetSpeakerImage(true);

        DisplayNodeOrQuit();

        //talkTimer = 0;
	}

	//SHOULD ONLY BE USED BY RESPONSE BUTTONS FOR NOW
	public void SelectResponse(int responseNode)
	{
		//Error check
		if (dialogueGraph.current.GetType() != null && dialogueGraph.current.GetType() != typeof(PromptNode))
		{
			Debug.LogError("Can't get responses because this isn't a PromptNode!");
			return;
		}

		PromptNode pNode = dialogueGraph.current as PromptNode;

		//Activate the listening image while I've got the emotion
		SetSpeakerImage(false);

		if (pNode.GetAnswerConnection(responseNode).throwFlag != FlagBank.Flags.NONE)
		{
			EventAnnouncer.OnThrowFlag?.Invoke(pNode.GetAnswerConnection(responseNode).throwFlag);
		}

		dialogueGraph.current = pNode.GetAnswerConnection(responseNode);//.GetNextNode();
		AssessCurrentType();

		//Activate the speaking for the new speaker
		SetSpeakerImage(true);

        DisplayNodeOrQuit();

        //talkTimer = 0;
	}
	#endregion

	//Used to enable object and start dialogue
	public void EnableCurrNode(SimpleGraph newGraph)
	{
		SwapGraph(newGraph);

		if (dialogueGraph.current != null)
		{
			AssessCurrentType();

			//SetSpeakerImage(false, 0);
			SetNeutralSpeakers();

			DisplayNode(dialogueGraph.current);
			gameObject.SetActive(true);

            //Start talking.
        }
	}

    public void DisableCharacterController(GameObject charContainer)
    {
        characterContainer = charContainer;

		if (dialogueGraph.current != null)
		{
			if (characterContainer != null)
			{
				characterContainer.SetActive(false);
			}
		}
    }

	private void SetNeutralSpeakers()
	{
		if (dialogueGraph.current.GetType() == null)
		{
			return;
		}

		List<Sprite> speakerPics = new List<Sprite>();
		int i = 0;
		for (; i < dialogueGraph.actors.Count; i++)
		{
			speakerPics.Add(dialogueGraph.GetSprite(false, dialogueGraph.actors[i], Character.EnumExpression.NEUTRAL));

			if (speakerPics[i] != null)
			{
				speakerImages[i].sprite = speakerPics[i];
				speakerImages[i].gameObject.SetActive(true); //should always be active
			}
		}
		for (; i < speakerImages.Count; i++)
		{
			speakerImages[i].gameObject.SetActive(false);
		}

		if (dialogueGraph.current.GetType() == typeof(PromptNode))
		{
			SetSpeakerImage(true);
		}
	}
	private void SetSpeakerImage(bool isSpeaking)//, int index)
	{
		//Error check
		if (dialogueGraph.current == null || dialogueGraph.current.GetType() != typeof(PromptNode))
		{
			return;
		}

		PromptNode pNode = dialogueGraph.current as PromptNode;

		Sprite speakerPic = dialogueGraph.GetSprite(isSpeaking, pNode.speaker, pNode.expression);
		if (speakerPic != null)
		{
			speakerImages[dialogueGraph.GetActorIndex(pNode.speaker)].sprite = speakerPic;
			//speakerImages[0].gameObject.SetActive(true); //should always be active, this line is depracated
		}
	}
	//Generally not used
	public void SetCurrNode(int newNode)
	{
		dialogueGraph.current = dialogueGraph.nodes[newNode] as BaseNode;
	}

	public void SwapGraph(SimpleGraph newGraph)
	{
		dialogueGraph = newGraph;

		if (!dialogueGraph.isInit)
		{
			Init();
		}
		if (dialogueGraph.current != null)
		{
			AssessCurrentType();

			SetNeutralSpeakers();
		}
	}

	//This checks to see what the type of the current node is, and evaluates until it reaches a PromptNode
	private void AssessCurrentType()
	{
		if (dialogueGraph.current == null)
		{
			Debug.Log("Null node");
			return;
		}

		while ((dialogueGraph.current.GetType() != typeof(PromptNode) && dialogueGraph.current.GetType() != typeof(ResponseNode)) && dialogueGraph.current != null)
		{
			//only supports branch nodes rn.
			if (dialogueGraph.current.GetType() == typeof(BranchNode))
			{
				BranchNode bNode = dialogueGraph.current as BranchNode;

				dialogueGraph.current = bNode.GetOutputNode();
			}
			else if (dialogueGraph.current.GetType() == typeof(ActorNode))
			{
				ActorNode aNode = dialogueGraph.current as ActorNode;

				if (aNode.status == ActorNode.ActorMovement.Arriving)
				{
					for (int i = 0; i < aNode.actors.Count; i++)
					{
						dialogueGraph.AddActor(aNode.actors[i]);
					}
				}
				else if (aNode.status == ActorNode.ActorMovement.Leaving)
				{
					for (int i = 0; i < aNode.actors.Count; i++)
					{
						dialogueGraph.RemoveActor(aNode.actors[i]);
					}
				}

				dialogueGraph.current = aNode.GetNextNode();
			}
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			if (this.enabled == true)
			{
				//TODO: setup interrupts in the xnode graph

				//the interrupt node
				//BaseNode interruptNode = pNode.GetConnectedNode("interruptConnection");
				//BaseNode checkpoint = pNode.GetConnectedNode("checkpointConnection");

				//set the interrupt node to go to the checkpoint node
				//This means interrupts can't ramble
				//dialogueGraph.nodes[interruptNode].connection = checkPointNode;
				//dialogueGraph.nodes[interruptNode].checkPointConnection = checkpoint;

				//now go to the interruptNode
				//dialogueGraph.current = interruptNode;// as PromptNode;
				this.enabled = false;
			}
		}
	}

	//A fun reset switch. SHOULD restart all the dialogue
	public void RestartAllDialogue()
	{
		Debug.Log("RESET");

		for (int i = 0; i < graphList.Count; i++)
		{
			graphList[i].isInit = false;
		}
	}

    private void DisplayNodeOrQuit()
    {
        if (dialogueGraph.current != null)
        {
            DisplayNode(dialogueGraph.current);
        }
        else
        {
            Debug.Log(ID + " is done talking");
			dialogueGraph.current = exitNode;

			gameObject.SetActive(false);
            characterContainer.SetActive(true);
        }
    }
}
