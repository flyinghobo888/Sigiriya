﻿using System.Collections;
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
    [SerializeField] BaseNode currNode;
	[SerializeField] PromptNode pNode;


	private string ID;
    private BaseNode checkPointNode = null;
    private BaseNode exitNode = null; 

	private float talkTimer = 0;

    private Vector3 worldToScreenPos;

    private void Awake()
    {
        ID = this.name;
		//EventManager.StartListening(ID + "_enable", EnableCurrNode);
		dialogueGraph.Restart();
		currNode = dialogueGraph.current as PromptNode;
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
		if (currNode.GetType() != typeof(PromptNode))
		{
			if (currNode.GetType() == typeof(BranchNode))
			{
				BranchNode bNode = currNode as BranchNode;
				
				currNode = bNode.GetOutputNode() as PromptNode;
			}
		}

		pNode = currNode as PromptNode;

		//-1 should be a signal to end the current discussion
		if (currNode != null && enabled)
        {
            //this can probs be moved out of update tbh
            DisplayNode(pNode);
        }
        else
        {
            Debug.Log(ID + " is done talking");
            gameObject.SetActive(false);

            currNode = exitNode as PromptNode;
        }

		talkTimer += Time.deltaTime;

		if (pNode.time == 0)
		{
			talkTimer = 0;
			Debug.Log("Node is missing a time!");
		}
		if (talkTimer > pNode.time)
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
		//TODO: fix this for the image swap from the character system
		if (pNode.speakerPic != null)
		{
			if (pNode.speakerPic == speakerImages[0].sprite)
			{
				speakerImages[0].gameObject.SetActive(true);
				speakerImages[1].gameObject.SetActive(false);
			}
			else if (pNode.speakerPic == speakerImages[1].sprite)
			{
				speakerImages[1].gameObject.SetActive(true);
				speakerImages[0].gameObject.SetActive(false);
			}
		}


		currNode = currNode.GetNextNode();

		talkTimer = 0;
	}

	public void SelectResponse(int responseNode)
    {
		//TODO: fix this for the image swap from the character system
		if (pNode.speakerPic != null)
		if (pNode.speakerPic == speakerImages[0].sprite)
		{
			speakerImages[0].gameObject.SetActive(true);
			speakerImages[1].gameObject.SetActive(false);
		}
		else if (pNode.speakerPic == speakerImages[1].sprite)
		{
			speakerImages[1].gameObject.SetActive(true);
			speakerImages[0].gameObject.SetActive(false);
		}


		if (pNode.GetAnswerConnection(responseNode).throwFlag != FlagBank.Flags.NONE)
        {
            EventAnnouncer.OnThrowFlag?.Invoke(pNode.GetAnswerConnection(responseNode).throwFlag);
        }

		currNode = pNode.GetAnswerConnection(responseNode).GetNextNode();

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
			DisplayNode(pNode);
			gameObject.SetActive(true);
		}
	}

    public void SetCurrNode(int newNode)
    {
		currNode = dialogueGraph.nodes[newNode] as BaseNode;// as PromptNode;
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
				currNode = interruptNode;// as PromptNode;
                this.enabled = false;
            }
        }
    }
}
