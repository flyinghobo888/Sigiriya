using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class used to read SimpleGraphs and control the dialogue UI
/// </summary>
public class DialogueController : ManagerBase<DialogueController>
{
	//Graph where nodes are kept
	public SimpleGraph dialogueGraph;
	[Header("Time")]
	//[SerializeField] private int days;
	//[SerializeField] private int hours;
	[SerializeField] private int minutesPerNode;
	[SerializeField] private int secondsPerNode;

	[SerializeField] private SigiTime timePerNode;

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
	private string ID;
	private BaseNode checkPointNode = null;
	private BaseNode exitNode = null;

	[Header("Pool")]
	[SerializeField] private List<SimpleGraph> dialoguePool = null;

    private void Init()
	{
		ID = name;

		dialogueGraph.Init();

		AssessCurrentType();
	}

	private void OnEnable()
	{
		//TODO: Save data somewhere, somehow, cause the dialogueGraph.current isn't actually saved on exit :(
		PersistentEventBank.FireAllEvents();
	}
	private void OnDisable()
	{
		//TODO: Save data somewhere, somehow, cause the dialogueGraph.current isn't actually saved on exit :(
		EventAnnouncer.OnDialogueEnd?.Invoke();
	}

	private void Start()
	{
        ActivateDialogueUI(false);

		//TODO: add days and hours for insane people
		timePerNode = new SigiTime(0, 0, minutesPerNode, secondsPerNode);
    }

	/// <summary>
	///A function to decide how to display the current node if it is a <see cref="PromptNode"/> or <see cref="ResponseNode"/>
	/// </summary>
	/// <param name="node"> Should cast <see cref="BaseNode"/> to <see cref="PromptNode"/> or <see cref="ResponseNode"/> </param>
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
					pNode.speaker.MoodTracker.AddMood(pNode.mood, new SigiTime(pNode.dys, pNode.hr, pNode.min, pNode.sec));
				}
				ActivateSpeechPrompt(EnumTalking.CHARACTER);
            }
            else
            {
                ActivateSpeechPrompt(EnumTalking.PLAYER);
            }

            currentPromptPanel.text = "";
            EventAnnouncer.OnDialogueUpdate?.Invoke(currentPromptPanel, pNode.prompt);

			if (pNode.isNoReturn)
			{
				dialogueGraph.isDone = true;
			}

			for (int i = 0; i < responseButtons.Length; i++)
			{
                responseButtons[i].gameObject.SetActive(false);
            }

			continueButton.gameObject.SetActive(true);
		}
		else if(node != null && node.GetType() == typeof(ResponseNode))
		{
			ResponseNode rNode = node as ResponseNode;

            //The player is responding
            ActivateSpeechPrompt(EnumTalking.PLAYER);
            currentPromptPanel.text = "";

            if (rNode.textFull != "")
            {
                EventAnnouncer.OnDialogueUpdate?.Invoke(currentPromptPanel, rNode.textFull);
            }
            else
            {
                EventAnnouncer.OnDialogueUpdate?.Invoke(currentPromptPanel, rNode.textButton);
            }

			Debug.Log("PLAYER TALKING");
			int i = 0;

			for (; i < responseButtons.Length; i++)
			{
				responseButtons[i].gameObject.SetActive(false);
			}

            continueButton.gameObject.SetActive(true);
        }
		else if (node != null && node.GetType() == typeof(ResponseBranchNode))
		{
			playerSpeechObj.SetActive(false);
			characterSpeechObj.SetActive(false);

			ResponseBranchNode rbNode = node as ResponseBranchNode;

			int i = 0;

			if (rbNode.responses.Count != 0) //if we have responses
			{
				int grossCount = 0; //part of the gross stuff
				for (; i < rbNode.responses.Count; i++)
				{
					//god this is so fucking gross

					//A check to see if visited
					if (rbNode.GetAnswerConnection(i).GetNextNode() != null && rbNode.GetAnswerConnection(i).GetNextNode().GetType() == typeof(PromptNode))
					{
						PromptNode ppNode = rbNode.GetAnswerConnection(i).GetNextNode() as PromptNode;

						if (ppNode.isVisited == true)
						{
							grossCount++;
							responseButtons[i].gameObject.SetActive(false);
							if (grossCount == rbNode.responses.Count)
							{
								continueButton.gameObject.SetActive(true);
							}
							continue;
						}
					}


					if (!rbNode.GetAnswerConnection(i).getHidden())
					{
						responseButtons[i].gameObject.SetActive(true);

						if (rbNode.GetAnswerConnection(i).textFull != "")
						{
							responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = rbNode.GetAnswerConnection(i).textFull;
						}
						else
						{
							responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = rbNode.GetAnswerConnection(i).textButton;
						}
					}
					else
					{
						responseButtons[i].gameObject.SetActive(false);
					}
				}

				if (grossCount != rbNode.responses.Count)
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

	}

	#region Continuation Commands
	/// <summary>
	/// Goes to the next default connected node
	/// </summary>
	public void ContinueDialogue()
	{
        if (TextController.Instance.TryToFinishText(currentPromptPanel))
        {
            return;
        }

		//Activate the listening image while I still have the emotion
		SetSpeakerImage(false);

		dialogueGraph.current = dialogueGraph.current.GetNextNode();
		AssessCurrentType();

		//activate the talking for the speaker
		SetSpeakerImage(true);

        DisplayNodeOrQuit();

		Debug.Log("Node Hit!");
	}

	public void TryFinishDialogue()
    {
        TextController.Instance.TryToFinishText(currentPromptPanel);
    }

	//SHOULD ONLY BE USED BY RESPONSE BUTTONS FOR NOW
	/// <summary>
	/// Goes to the next node on a <see cref="ResponseNode"/>
	/// </summary>
	/// <param name="responseNode"> Index of the <see cref="ResponseNode"/> </param>
	public void SelectResponse(int responseNode)
	{
		//Error check
		if (dialogueGraph.current.GetType() != null && dialogueGraph.current.GetType() != typeof(ResponseBranchNode))
		{
			Debug.LogError("Can't get responses because this isn't a PromptNode!");
			return;
		}

		ResponseBranchNode rbNode = dialogueGraph.current as ResponseBranchNode;

		//Activate the listening image while I've got the emotion
		SetSpeakerImage(false);

		//if (pNode.GetAnswerConnection(responseNode).throwFlag != FlagBank.Flags.NONE)
		//{
		//	EventAnnouncer.OnThrowFlag?.Invoke(pNode.GetAnswerConnection(responseNode).throwFlag);
		//}

		dialogueGraph.current = rbNode.GetAnswerConnection(responseNode);
		AssessCurrentType();

		//Activate the speaking for the new speaker
		SetSpeakerImage(true);

        DisplayNodeOrQuit();

		Debug.Log("Node Hit!");
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
			foreach (SimpleGraph graph in dialoguePool)
			{
				//cull the impossible

				//check required graphs
				bool containsGraph = true;
				for (int i = 0; i < graph.dependantConversation.Count; i++)
				{
					if (dialoguePool.Contains(graph.dependantConversation[i]))
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
				if (graph.timeOfDay.Count > 0 && !graph.timeOfDay.Contains(GlobalTimeTracker.Instance.CurrentTimeOfDay))
				{
					continue;
				}

				//check location 
				if (graph.location.Count > 0 && !graph.location.Contains(LocationTracker.Instance.TargetLocation))
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
			if (possibleGraphs.Count > 1)
			{
				int num = Random.Range(0, possibleGraphs.Count);

				dialogueGraph = possibleGraphs[num];
			}
			else if (possibleGraphs.Count == 1)
			{
				dialogueGraph = possibleGraphs[0];
			}
			else
			{
				dialogueGraph = null;
				return;
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

			SetNeutralSpeakers();
            BringAllToForeground();

			DisplayNode(dialogueGraph.current);
            ActivateDialogueUI(true);
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

			SetNeutralSpeakers();
			BringAllToForeground();

			DisplayNode(dialogueGraph.current);
			ActivateDialogueUI(true);
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

	private void SetSpeakerImage(bool isSpeaking)
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

		while (dialogueGraph.current != null && (dialogueGraph.current.GetType() != typeof(PromptNode) && dialogueGraph.current.GetType() != typeof(ResponseNode) && dialogueGraph.current.GetType() != typeof(ResponseBranchNode)))
		{
			if (dialogueGraph.current.GetType() == typeof(BranchNode))
			{
				BranchNode bNode = dialogueGraph.current as BranchNode;

				dialogueGraph.current = bNode.GetOutputNode();
				Debug.Log("Node Hit!");
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
				Debug.Log("Node Hit!");
			}
			else if (dialogueGraph.current.GetType() == typeof(FlagNode))
			{
				FlagNode fNode = dialogueGraph.current as FlagNode;

				if (fNode.throwFlag != FlagBank.Flags.NONE)
				{
					EventAnnouncer.OnThrowFlag?.Invoke(fNode.throwFlag);
				}

				dialogueGraph.current = fNode.GetNextNode();
				Debug.Log("Node Hit!");
			}
			else if (dialogueGraph.current.GetType() == typeof(MemoryNode))
			{
				MemoryNode mNode = dialogueGraph.current as MemoryNode;

				if (mNode.memory != null)
				{
					Debug.Log(mNode.memory.memoryImage.name);
					MemoryManager.Instance.AddMemory(mNode.memory);
				}

				dialogueGraph.current = mNode.GetNextNode();
				Debug.Log("Node Hit!");
			}
		}

		if (dialogueGraph.current.GetType() == typeof(PromptNode))
		{
			PromptNode pNode = dialogueGraph.current as PromptNode;

			pNode.isVisited = true;
		}
	}

	//A fun reset switch. SHOULD restart all the dialogue
	public void RestartAllDialogue()
	{
		Debug.Log("RESET");

		for (int i = 0; i < dialoguePool.Count; i++)
		{
			dialoguePool[i].Reset();
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
			dialogueGraph.ResetIsVisited();

			if (dialogueGraph.isDone)
			{
				dialoguePool.Remove(dialogueGraph);
			}

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
