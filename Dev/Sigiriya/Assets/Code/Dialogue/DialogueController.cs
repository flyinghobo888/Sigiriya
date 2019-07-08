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

public class DialogueController : MonoBehaviour
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
	[SerializeField] private List<Image> speakerImages; //basically just a holder of images to display 

	[Header("Dialogue")]
	[SerializeField] PromptNode pNode;
	private string ID;
	private BaseNode checkPointNode = null;
	private BaseNode exitNode = null;

	private float talkTimer = 0;

	[Header("Dev/Editor")]
	[SerializeField] private List<SimpleGraph> graphList;

	private void Awake()
	{
		if (dialogueGraph.isInit)
		{
			Init();
		}
	}

	private void Init()
	{
		ID = this.name;
		//EventManager.StartListening(ID + "_enable", EnableCurrNode);
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
		//null should be a signal to end the current discussion
		if (dialogueGraph.current != null && enabled)
		{
			//this can probs be moved out of update tbh
			DisplayNode(pNode);
		}
		else
		{
			Debug.Log(ID + " is done talking");
			gameObject.SetActive(false);

			dialogueGraph.current = exitNode;
		}

		if (pNode.time != 0)
		{
			talkTimer += Time.deltaTime;
		}
		if (talkTimer > pNode.time)
		{
			ContinueDialogue();

			talkTimer = 0;
		}
	}

	//Display node can only display PromptNodes. This might need to also support ResponseNodes
	void DisplayNode(PromptNode node)
	{
		checkPointNode = node.GetConnectedNode("checkpointConnection");
		exitNode = node.GetConnectedNode("exitConnection");

		//nameBox.text = node.speaker==null ? "Player" : node.speaker.characterName;
		promptPanel.text = node.prompt;

		//Activate the talking sprite 
		SetSpeakerImage(true);

		int i = 0;

		if (node.responses != null) //if we have responses
		{
			for (; i < node.responses.Count; i++)
			{
				if (!node.GetAnswerConnection(i).getHidden())
				{
					responseButtons[i].gameObject.SetActive(true);
					//TODO: update the response class with two(2) text things. a button text, and full text
					responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = node.GetAnswerConnection(i).textButton;
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

	#region Continuation Commands
	//goes to the next default connected node
	public void ContinueDialogue()
	{
		//Activate the listening image
		SetSpeakerImage(false);

		dialogueGraph.current = dialogueGraph.current.GetNextNode();

		AssessCurrentType();
		talkTimer = 0;
	}

	//SHOULD ONLY BE USED BY RESPONSE BUTTONS FOR NOW
	public void SelectResponse(int responseNode)
	{
		//Activate the listening image
		SetSpeakerImage(false);

		if (pNode.GetAnswerConnection(responseNode).throwFlag != FlagBank.Flags.NONE)
		{
			EventAnnouncer.OnThrowFlag?.Invoke(pNode.GetAnswerConnection(responseNode).throwFlag);
		}

		dialogueGraph.current = pNode.GetAnswerConnection(responseNode).GetNextNode();

		AssessCurrentType();

		talkTimer = 0;
	}
	#endregion

	//Used to enable object and start dialogue
	public void EnableCurrNode(SimpleGraph newGraph)
	{
		SwapGraph(newGraph);

		if (!dialogueGraph.isInit)
		{
			Init();
		}
		if (dialogueGraph.current != null)
		{
			AssessCurrentType();

			pNode = dialogueGraph.current as PromptNode;
			SetSpeakerImage(false);

			DisplayNode(pNode);
			gameObject.SetActive(true);
		}
	}

	//Generally not used
	public void SetCurrNode(int newNode)
	{
		dialogueGraph.current = dialogueGraph.nodes[newNode] as BaseNode;
	}

	private void SetSpeakerImage(bool isSpeaking)
	{
		Sprite speakerPic = pNode.GetSprite(isSpeaking);
		if (speakerPic != null)
		{
			speakerImages[0].sprite = speakerPic;
			speakerImages[0].gameObject.SetActive(true);
		}
	}

	public void SwapGraph(SimpleGraph newGraph)
	{
		dialogueGraph = newGraph;

		if (!dialogueGraph.isInit)
		{
			Init();
		}

		AssessCurrentType();

		SetSpeakerImage(false);

		//Problem where on graph swap speakerimage isn't updated
		//This revealed another underlying problem where the speakers don't show up until they speak
		//Which in turn is a major barrier to multi character talks
		//TODO: Figure out a more graceful way to solve the image swapping
		speakerImages[0].gameObject.SetActive(false);
	}

	//This checks to see what the type of the current node is, and evaluates until it reaches a PromptNode
	private void AssessCurrentType()
	{
		if (dialogueGraph.current == null)
		{
			Debug.Log("Null node");
			return;
		}

		while (dialogueGraph.current.GetType() != typeof(PromptNode) && dialogueGraph.current != null)
		{
			//only supports branch nodes rn.
			if (dialogueGraph.current.GetType() == typeof(BranchNode))
			{
				BranchNode bNode = dialogueGraph.current as BranchNode;

				dialogueGraph.current = bNode.GetOutputNode();
			}
			//TODO: This needs to be actually supported down the road
			else if (dialogueGraph.current.GetType() == typeof(ResponseNode))
			{
				ResponseNode rNode = dialogueGraph.current as ResponseNode;

				dialogueGraph.current = rNode.GetNextNode();
			}
		}

		pNode = dialogueGraph.current as PromptNode;
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			if (this.enabled == true)
			{
				//the interrupt node
				BaseNode interruptNode = pNode.GetConnectedNode("interruptConnection");
				BaseNode checkpoint = pNode.GetConnectedNode("checkpointConnection");

				//TODO: setup interrupts in the xnode graph
				//set the interrupt node to go to the checkpoint node
				//This means interrupts can't ramble
				//dialogueGraph.nodes[interruptNode].connection = checkPointNode;
				//dialogueGraph.nodes[interruptNode].checkPointConnection = checkpoint;

				//now go to the interruptNode
				dialogueGraph.current = interruptNode;// as PromptNode;
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
}
