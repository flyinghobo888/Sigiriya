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
    [SerializeField] private TextMeshProUGUI promptPanel = null;
    [SerializeField] private Button[] responseButtons = null;
    [SerializeField] private Button continueButton = null;
	[SerializeField] private List<Image> speakerImages; //basically just a holder of images to display 

    [Header("Dialogue")]
    //[SerializeField] public List<DialogueNode> nodes;
    [SerializeField] PromptNode currNode;


	private string ID;
    private PromptNode checkPointNode = null;
    private PromptNode exitNode = null; 

	private float talkTimer = 0;

    private Vector3 worldToScreenPos;

    [System.Serializable]
    public class DialogueNode
    {  
		//Box for the prompt and the text to go inside
        [Header("Prompt Info")]
        public string prompt;
		public float time;

        [Header("Response Info")]
        [SerializeField] public List<Response> responses;

        //what the next dialogue node is. -1 to terminate discussion
        //should be one for each button, or only one if no buttons

        [Header("Connection Info")]
        //[SerializeField] public int[] connections;
        [SerializeField] public int connection;

        [Header("Special Connections")]
        [SerializeField] public int interruptConnection;
        [SerializeField] public int checkPointConnection;
        [SerializeField] public int exitConnection;

        [Header("Image")]
        [SerializeField] public tempCharSprites speakerPic; //This might one day become a list... :(
		[SerializeField] public Sprite usedSprite;
    };

    //Boxs for responses and the text inside them
    [System.Serializable]
    public class Response
    {
        [SerializeField] public string response;
        [SerializeField] public int connection;
        [Header("Flags")]
        [SerializeField] public string throwFlag;
        [SerializeField] public string catchFlag;
        [SerializeField] public bool hidden;

        public void CheckFlag(string flag)
        {
            if (flag == catchFlag)
            {
                hidden = false;
            }
        }
    };

    private void Awake()
    {
        ID = this.name;
		//EventManager.StartListening(ID + "_enable", EnableCurrNode);
		dialogueGraph.Restart();
		currNode = dialogueGraph.current as PromptNode;

		//Go through each node in the node graph and set up this classes information from the graph
		//for (int i = 0; i < graph.nodes.Count; i++)
		//{
		//	BaseNode node = graph.nodes[i] as BaseNode;
		//
		//	nodes.Add(new DialogueNode());
		//	nodes[i].prompt = node.prompt;
		//	nodes[i].speakerPic = node.speakerPic;
		//	nodes[i].usedSprite = node.usedSprite;
		//
		//	if (node.GetNextNode() != null)
		//		nodes[i].connection = node.GetNextNode().GetIndex();
		//	else
		//		nodes[i].connection = -1;
		//
		//	if (node.GetConnectedNode("interruptConnection") != null)
		//		nodes[i].interruptConnection = node.GetConnectedNode("interruptConnection").GetIndex();
		//	else
		//		nodes[i].interruptConnection = -1;
		//
		//	if (node.GetConnectedNode("checkPointConnection") != null)
		//		nodes[i].checkPointConnection = node.GetConnectedNode("checkPointConnection").GetIndex();
		//	else
		//		nodes[i].checkPointConnection = -1;
		//
		//	if (node.GetConnectedNode("exitConnection") != null)
		//		nodes[i].exitConnection = node.GetConnectedNode("exitConnection").GetIndex();
		//	else
		//		nodes[i].exitConnection = -1;
		//
		//	nodes[i].time = node.time;
		//
		//	if (node.answers.Count > 0) 
		//	{
		//		//Initialize list if we haven't already
		//		if (nodes[i].responses == null)
		//		{
		//			nodes[i].responses = new List<Response>();
		//		}
		//
		//		//For each response, fill in the data
		//		for (int j = 0; j < node.answers.Count; j++)
		//		{
		//			nodes[i].responses.Add(new Response());
		//			nodes[i].responses[j].response = node.answers[j].text;
		//			nodes[i].responses[j].connection = node.GetAnswerConnection(j).GetIndex();
		//			nodes[i].responses[j].hidden = node.answers[j].isHidden;
		//			if (node.answers[j].throwFlag != "")
		//			{
		//				nodes[i].responses[j].throwFlag = node.answers[j].throwFlag;
		//			}
		//			if (node.answers[j].catchFlag != "")
		//			{
		//				nodes[i].responses[j].catchFlag = node.answers[j].catchFlag;
		//			}
		//		}
		//	}
		//	else if (node.GetNextNode() == null)
		//	{
		//		nodes[i].connection = -1;
		//	}
		//}

	}

    private void OnEnable()
    {
		//EventManager.StartListening("E_down", ContinueDialogue);
		//EventManager.FireEvent("MENU_open");

		for (int i = 0; i < dialogueGraph.nodes.Count; i++)
        {
			//Debug.Log(dialogueGraph.nodes[i].GetType());

			if (dialogueGraph.nodes[i].GetType() == typeof(PromptNode))
			{
				PromptNode node = dialogueGraph.nodes[i] as PromptNode;

				if (node.responses != null)
				{
					for (int j = 0; j < node.responses.Count; j++)
					{
						//EventAnnouncer.OnThrowFlag += node.responses[j].CheckFlag;
					}
				}
			}
        }

        PersistentEventBank.FireAllEvents();
    }
    private void OnDisable()
    {
        //EventManager.StopListening("E_down", ContinueDialogue);
        //EventManager.FireEvent("MENU_close");

        //TODO: THIS IS FOR OLD PROTOTYPE PLZ FIX
        ///PlayerPrefs.SetInt(Managers.GameStateManager.Instance.CurrentTime + gameObject.name, currNode);

        for (int i = 0; i < dialogueGraph.nodes.Count; i++)
        {
			if (dialogueGraph.nodes[i].GetType() == typeof(PromptNode))
			{
				PromptNode node = dialogueGraph.nodes[i] as PromptNode;

				if (node.responses != null)
				{
					for (int j = 0; j < node.responses.Count; j++)
					{
						//EventAnnouncer.OnThrowFlag -= node.GetAnswerConnection(j).CheckFlag;
					}
				}
			}
        }

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
		//-1 should be a signal to end the current discussion
		if (currNode != null && enabled)
        {
            //this can probs be moved out of update tbh
            DisplayNode(currNode);
        }
        else
        {
            Debug.Log(ID + " is done talking");
            gameObject.SetActive(false);

            currNode = exitNode;
        }

		talkTimer += Time.deltaTime;

		if (currNode.time == 0)
		{
			talkTimer = 0;
			Debug.Log("Node is missing a time!");
		}
		if (talkTimer > currNode.time)
		{
			if (currNode.GetNextNode() != null && enabled)
			{
				ContinueDialogue();
			}
			else
			{
				Debug.Log(ID + " is done talking");
				gameObject.SetActive(false);

				currNode = exitNode;
			}

			talkTimer = 0;
		}
    }

	void DisplayNode(PromptNode node)
	{
		checkPointNode = node.GetConnectedNode("checkpointConnection");
		exitNode = node.GetConnectedNode("exitConnection");

		promptPanel.text = node.prompt;

		/*
		 * if the speakerImage.sprite exists in the nodes.speakerPic list, go nuts. else, other way around
		 */
		if (node.speakerPic != null)
		{
			if (node.speakerPic.spriteList.Contains(speakerImages[0].sprite))
			{
				speakerImages[0].sprite = node.usedSprite;
				speakerImages[0].gameObject.SetActive(true);
				// only one speaker right now. Will need to expand this later
				//speakerImages[1].gameObject.SetActive(false);
			}
		}
		int i = 0;

        if (node.responses != null) //if we have responses
        {
            for (; i < node.responses.Count; i++)
            {
                if (!node.GetAnswerConnection(i).getHidden())
                {
                    responseButtons[i].gameObject.SetActive(true);
                    responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = node.GetAnswerConnection(i).text;
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

    #region Button Commands
    //exposed to the UI continue button
    public void ContinueDialogue()
    {
		//if (nodes[currNode].speakerPic != null)
		//{
		//    nodes[currNode].speakerPic.gameObject.SetActive(false);
		//}
		if (currNode.speakerPic != null)
		{
			if (currNode.speakerPic == speakerImages[0].sprite)
			{
				speakerImages[0].gameObject.SetActive(true);
				speakerImages[1].gameObject.SetActive(false);
			}
			else if (currNode.speakerPic == speakerImages[1].sprite)
			{
				speakerImages[1].gameObject.SetActive(true);
				speakerImages[0].gameObject.SetActive(false);
			}
		}

		currNode = currNode.GetNextNode() as PromptNode;

		talkTimer = 0;
	}

	public void SelectResponse(int responseNode)
    {
        //if (nodes[currNode].speakerPic != null)
        //{
        //    nodes[currNode].speakerPic.gameObject.SetActive(false);
        //}

		if (currNode.speakerPic != null)
		if (currNode.speakerPic == speakerImages[0].sprite)
		{
			speakerImages[0].gameObject.SetActive(true);
			speakerImages[1].gameObject.SetActive(false);
		}
		else if (currNode.speakerPic == speakerImages[1].sprite)
		{
			speakerImages[1].gameObject.SetActive(true);
			speakerImages[0].gameObject.SetActive(false);
		}


		if (currNode.GetAnswerConnection(responseNode).throwFlag != FlagBank.Flags.NONE)
        {
            EventAnnouncer.OnThrowFlag?.Invoke(currNode.GetAnswerConnection(responseNode).throwFlag);
        }

        currNode = currNode.GetAnswerConnection(responseNode).GetNextNode() as PromptNode;

		talkTimer = 0;
	}

	#endregion

	public void EnableCurrNode()
    {
		//currNode = checkPointNode;
		//currNode = exitNode;

		//TODO: THIS IS FOR OLD PROTOTYPE PLZ FIX
		///currNode = PlayerPrefs.GetInt(Managers.GameStateManager.Instance.CurrentTime + gameObject.name, 0);

		if (currNode != null)
        {
			DisplayNode(currNode);
			gameObject.SetActive(true);
		}
	}

    public void SetCurrNode(int newNode)
    {
        currNode = dialogueGraph.nodes[newNode] as PromptNode;
    }

    private void OnTriggerExit(Collider col)
    {
		if (col.tag == "Player")
        {
            if (this.enabled == true)
            {
				//the interrupt node
				PromptNode interruptNode = currNode.GetConnectedNode("interruptConnection");
				PromptNode checkpoint = currNode.GetConnectedNode("checkpointConnection");

				//TODO: setup interrupts in the xnode graph
				//set the interrupt node to go to the checkpoint node
                //This means interrupts can't ramble
                //dialogueGraph.nodes[interruptNode].connection = checkPointNode;
				//dialogueGraph.nodes[interruptNode].checkPointConnection = checkpoint;

                //now go to the interruptNode
                currNode = interruptNode;
                this.enabled = false;
            }
        }
    }
}
