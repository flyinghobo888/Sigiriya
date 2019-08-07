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

/// <summary>
/// Class used to read SimpleGraphs and control the dialogue UI
/// </summary>
public class DialogueController : ManagerBase<DialogueController>
{
	//[Header("Camera")]
	//[SerializeField] private int textSize;

	//Graph where nodes are kept
	public SimpleGraph dialogueGraph;

    [Header("Dialogue Scene Objects")]
    [SerializeField] private GameObject dialogueContainerObj = null;
    [SerializeField] private GameObject backgroundBlurContainer = null;
	[SerializeField] private Button[] responseButtons = null;
	[SerializeField] private Button continueButton = null;
	[SerializeField] private List<Image> speakerImages = null; //basically just a holder of images to display

    [Header("Player UI Fields")]
    [SerializeField] private GameObject playerSpeechObj = null;
    [SerializeField] private TextMeshProUGUI playerPromptPanel = null;

    [Header("Character UI Fields")]
    [SerializeField] private GameObject characterSpeechObj = null;
    [SerializeField] private TextMeshProUGUI characterNameBox = null;
	[SerializeField] private TextMeshProUGUI characterPromptPanel = null;

    [Header("Foreground and Background Fields")]
    [SerializeField] private GameObject backgroundContainer = null;
    [SerializeField] private GameObject foregroundContainer = null;

    private TextMeshProUGUI currentPromptPanel = null;
    private GameObject characterButtonObj = null;

    private GameObject prevSpeakingCharacter = null;
    private GameObject currentSpeakingCharacter = null;

	[Header("Dialogue")]
	//[SerializeField] PromptNode pNode;
	private string ID;
	private BaseNode checkPointNode = null;
	private BaseNode exitNode = null;

	//private float talkTimer = 0;

	[Header("Dev/Editor")]
	[SerializeField] private List<SimpleGraph> graphList = null;

    //speakerImages[dialogueGraph.GetActorIndex(pNode.speaker)].gameObject - gets the game object in the scene of who is currently speaking. If pNode.speaker is null, the player is talking

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
		ID = name;

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
        //dialogueContainerObj.SetActive(false);
        //backgroundBlurContainer.SetActive(false);
        ActivateDialogueUI(false);
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

	///<summary> A function to decide how to display the current node </summary>
	void DisplayNode(BaseNode node)
	{
		if (node != null && node.GetType() == typeof(PromptNode))
		{
			PromptNode pNode = node as PromptNode;

			checkPointNode = pNode.GetConnectedNode("checkpointConnection");
			exitNode = pNode.GetConnectedNode("exitConnection");

            //The character is speaking
            if (pNode.speaker != null)
            {
                characterNameBox.text = pNode.speaker.characterName;
				if (pNode.mood != EnumMood.NONE)
				{
					pNode.speaker.MoodTracker.AddMood(pNode.mood, pNode.moodDuration);
				}
				ActivateSpeechPrompt(EnumTalking.CHARACTER);
            }
            else
            {
                ActivateSpeechPrompt(EnumTalking.PLAYER);
            }

            currentPromptPanel.text = "";
            EventAnnouncer.OnDialogueUpdate(currentPromptPanel, pNode.prompt);

            //nameBox.text = pNode.speaker == null ? "Player" : pNode.speaker.characterName;
            //promptPanel.text = "";//pNode.prompt;
            //EventAnnouncer.OnDialogueUpdate(promptPanel, pNode.prompt);

			if (pNode.isNoReturn)
			{
				Debug.Log("YOU. SHALL NOT. PAAAASSSSSSSS!!! but i dunno what to do");
			}

            int i = 0;

			if (pNode.responses.Count != 0) //if we have responses
			{
				int grossCount = 0; //part of the gross stuff
                for (; i < pNode.responses.Count; i++)
				{
					//god this is so fucking gross
					//TODO: hey, a fix might be to add a function in responses to check their connected promptNode,
					//IF it's a prompt node
					//A check to see if visited
					if (pNode.GetAnswerConnection(i).GetNextNode() != null && pNode.GetAnswerConnection(i).GetNextNode().GetType() == typeof(PromptNode))
					{
						PromptNode ppNode = pNode.GetAnswerConnection(i).GetNextNode() as PromptNode;

						if (ppNode.isVisited == true)
						{
							grossCount++;
							responseButtons[i].gameObject.SetActive(false);
							if(grossCount == pNode.responses.Count)
							{
								continueButton.gameObject.SetActive(true);
							}
							continue;
						}
					}


					if (!pNode.GetAnswerConnection(i).getHidden())
                    {
                        responseButtons[i].gameObject.SetActive(true);
                        responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = pNode.GetAnswerConnection(i).textButton;
                    }
					else
					{
                        responseButtons[i].gameObject.SetActive(false);
                    }
                }

				if (grossCount != pNode.responses.Count)
				{
					continueButton.gameObject.SetActive(false);
				}
			}
			else
			{
				continueButton.gameObject.SetActive(true);
			}

			for (; i < responseButtons.Length; i++)
			{
				responseButtons[i].gameObject.SetActive(false);
			}
		}
		else if(node != null && node.GetType() == typeof(ResponseNode))
		{
			ResponseNode rNode = node as ResponseNode;

            //The player is responding
            ActivateSpeechPrompt(EnumTalking.PLAYER);
            currentPromptPanel.text = "";

            if (rNode.textFull != "")
            {
                EventAnnouncer.OnDialogueUpdate(currentPromptPanel, rNode.textFull);
            }
            else
            {
                EventAnnouncer.OnDialogueUpdate(currentPromptPanel, rNode.textButton);
            }

            //nameBox.text = "Player";
            //if (rNode.textFull != "")
			//{
                //promptPanel.text = "";//rNode.textFull;
                //EventAnnouncer.OnDialogueUpdate(promptPanel, rNode.textFull);
            //}
			//else
			//{
                //promptPanel.text = "";//rNode.textButton;
                //EventAnnouncer.OnDialogueUpdate(promptPanel, rNode.textButton);
            //}

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
	/// <summary>
	/// Goes to the next default connected node
	/// </summary>
	public void ContinueDialogue()
	{
        if (!TextController.Instance.IsFinished)
        {
            EventAnnouncer.OnDialogueRequestFinish(currentPromptPanel);
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
	/// <summary>
	/// Goes to the next node on a <see cref="ResponseNode"/>
	/// </summary>
	/// <param name="responseNode"> Index of the <see cref="ResponseNode"/> </param>
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

	/// <summary>
	/// Used to enable object and start dialogue. Decides graph to use internally.
	/// </summary>
	/// <param name="character"> List of <see cref="Character"/> objects that will  be speaking. </param>
	public void EnableCurrNode(List<Character> character)
	{
		//TODO: Maybe move this decision logic to the buttonInfo script, since the bubble thing won't work anymore
		if (character != null)
		{
			List<SimpleGraph> possibleGraphs = new List<SimpleGraph>();
			foreach (SimpleGraph graph in graphList)
			{
				//cull the impossible

				//check required graphs
				bool containsGraph = true;
				for (int i = 0; i < graph.dependantConversation.Count; i++)
				{
					if (graphList.Contains(graph.dependantConversation[i]))
					{
						containsGraph = false;
					}
				}
				if (!containsGraph)
				{
					continue;
				}

				//check characters
				if (character.Count != graph.actors.Count)
				{
					continue; //fails
				}

				bool containsActor = true;
				for (int i = 0; i < character.Count; i++)
				{
					if (!graph.actors.Contains(character[i]))
					{
						containsActor = false; //fails
					}
				}
				if (!containsActor)
				{
					continue;
				}

				//check time
				if (!graph.timeOfDay.Contains(GlobalTimeTracker.Instance.CurrentTimeOfDay))
				{
					continue;
				}

				//check location 
				if (!graph.location.Contains(LocationTracker.Instance.TargetLocation))
				{
					continue;
				}

				//check flags
				bool containsFlags = true;
				for (int i = 0; i < graph.neededFlags.Count; i++)
				{
					if (!PersistentEventBank.ContainsFlag(graph.neededFlags[i]))
					{
						containsFlags = false;
					}
				}
				if (!containsFlags)
				{
					continue;
				}

				//congrats! you passed!
				possibleGraphs.Add(graph);
			}
			//Second Pass. ie. out of the possible convos, what do we pick?
			foreach (SimpleGraph graph in possibleGraphs)
			{
				//Does nothing rn
			}
			//Now if we have more than one in possibleGraphs
			if (possibleGraphs.Count > 0)
			{
				int num = Random.Range(0, possibleGraphs.Count - 1);

				dialogueGraph = possibleGraphs[num];
			}
			else if (possibleGraphs.Count == 0)
			{
				dialogueGraph = possibleGraphs[0];
			}
			else
			{
				dialogueGraph = null;
			}
		}
		else
		{
			return;
		}

		SwapGraph(dialogueGraph);

		if (dialogueGraph.current != null)
		{
			AssessCurrentType();

			//SetSpeakerImage(false, 0);
			SetNeutralSpeakers();
            BringAllToForeground();

			DisplayNode(dialogueGraph.current);
            ActivateDialogueUI(true);
			//dialogueContainerObj.SetActive(true);
			dialogueGraph.timesAccessed++;
			//Start talking.
		}
	}
	/// <summary>
	/// Used to enable object and start dialogue. Uses graph provided.
	/// </summary>
	/// <param name="newGraph"> A <see cref="SimpleGraph"/> that is used for dialogue </param>
	public void EnableCurrNode(SimpleGraph newGraph)
	{
		SwapGraph(newGraph);

		if (dialogueGraph.current != null)
		{
			AssessCurrentType();

			//SetSpeakerImage(false, 0);
			SetNeutralSpeakers();
			BringAllToForeground();

			DisplayNode(dialogueGraph.current);
			ActivateDialogueUI(true);
			//dialogueContainerObj.SetActive(true);
			dialogueGraph.timesAccessed++;
			//Start talking.
		}
	}

	/// <summary>
	/// Disables UI scene object when diaologue UI opens
	/// </summary>
	/// <param name="charContainer"> An object holding all desired UI elements </param>
	public void DisableCharacterController(GameObject charContainer)
    {
        characterButtonObj = charContainer;

		if (dialogueGraph != null)
		{
			if (dialogueGraph.current != null)
			{
				if (characterButtonObj != null)
				{
					characterButtonObj.SetActive(false);
				}
			}
		}
    }

	private void SetNeutralSpeakers()
	{
		if (dialogueGraph.current == null)
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
        UpdateSpeakingCharacter(ref pNode);

        Sprite speakerPic = dialogueGraph.GetSprite(isSpeaking, pNode.speaker, pNode.expression);
		if (speakerPic != null)
		{
            Image speakerImage = speakerImages[dialogueGraph.GetActorIndex(pNode.speaker)];
            speakerImage.sprite = speakerPic;

            //speakerImage.rectTransform.sizeDelta = speakerPic.bounds.reatio;

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

		while (dialogueGraph.current != null && (dialogueGraph.current.GetType() != typeof(PromptNode) && dialogueGraph.current.GetType() != typeof(ResponseNode)))
		{
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

		if (dialogueGraph.current.GetType() == typeof(PromptNode))
		{
			PromptNode pNode = dialogueGraph.current as PromptNode;

			pNode.isVisited = true;
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			if (enabled)
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
				enabled = false;
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

        EventAnnouncer.OnDialogueRestart?.Invoke();
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

            ActivateDialogueUI(false);

            if (characterButtonObj)
            {
                characterButtonObj.SetActive(true);
            }
        }
    }

    private void ActivateDialogueUI(bool activate)
    {
        backgroundBlurContainer.SetActive(activate);
        dialogueContainerObj.SetActive(activate);
    }

    private void ActivateSpeechPrompt(EnumTalking whoIsTalking)
    {
        if (whoIsTalking == EnumTalking.PLAYER)
        {
            playerSpeechObj.SetActive(true);
            characterSpeechObj.SetActive(false);
            currentPromptPanel = playerPromptPanel;
        }
        else
        {
            playerSpeechObj.SetActive(false);
            characterSpeechObj.SetActive(true);
            currentPromptPanel = characterPromptPanel;
        }
    }

    //If prevSpeakingCharacter or currentSpeakingCharacter is null, that means it's the player.
    private void UpdateSpeakingCharacter(ref PromptNode pNode)
    {
        if (currentSpeakingCharacter)
        {
            prevSpeakingCharacter = currentSpeakingCharacter;
        }

        //The player is talking.
        if (pNode.speaker != null)
        {
            currentSpeakingCharacter = speakerImages[dialogueGraph.GetActorIndex(pNode.speaker)].gameObject;
        }

        BringAllToBackground();
        UpdateSpeakingCharacterLocation();
    }

    private void UpdateSpeakingCharacterLocation()
    {
        if (prevSpeakingCharacter && !currentSpeakingCharacter)
        {
            prevSpeakingCharacter.transform.SetParent(backgroundContainer.transform);
        }

        if (currentSpeakingCharacter)
        {
            currentSpeakingCharacter.transform.SetParent(foregroundContainer.transform);
        }
    }

    private void BringAllToForeground()
    {
        foreach (Image speaker in speakerImages)
        {
            speaker.transform.SetParent(foregroundContainer.transform);
            speaker.transform.SetAsFirstSibling();
        }
    }

    private void BringAllToBackground()
    {
        foreach (Image speaker in speakerImages)
        {
            speaker.transform.SetParent(backgroundContainer.transform);
            speaker.transform.SetAsFirstSibling();
        }
    }

    private enum EnumTalking
    {
        PLAYER,
        CHARACTER
    }
}
