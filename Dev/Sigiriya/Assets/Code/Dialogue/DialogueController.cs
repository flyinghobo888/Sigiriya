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

	[Header("UI Fields")]
	[SerializeField] TextMeshProUGUI promptPanel;
	[SerializeField] Button[] responseButtons;
	[SerializeField] Button continueButton;
	//private Text[] responseButtonText; 

	[Header("Dialogue")]
	[SerializeField] public DialogueNode[] nodes;
	[SerializeField] int currNode = 0;

	string ID;
	int checkPointNode = 0;
	int startNode = 0;
	int exitNode = 2000; //HACK: a temporary number

	Vector3 worldToScreenPos;

	[System.Serializable]
	public class DialogueNode
	{
		//Box for the prompt and the text to go inside
		[Header("Prompt Info")]
		public string prompt;

		[Header("Response Info")]
		[SerializeField] public Response[] responses;

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
		[SerializeField] public Image speakerPic;
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

	}

	private void OnEnable()
	{
		//EventManager.StartListening("E_down", ContinueDialogue);
		//EventManager.FireEvent("MENU_open");

		for (int i = 0; i < nodes.Length; i++)
		{
			for (int j = 0; j < nodes[i].responses.Length; j++)
			{
				Managers.EventManager.OnThrowFlag += nodes[i].responses[j].CheckFlag;
			}
		}

		DontDestroy.FireAllEvents();
	}
	private void OnDisable()
	{
		//EventManager.StopListening("E_down", ContinueDialogue);
		//EventManager.FireEvent("MENU_close");

		PlayerPrefs.SetInt(Managers.GameStateManager.Instance.CurrentTime + gameObject.name, currNode);

		for (int i = 0; i < nodes.Length; i++)
		{
			for (int j = 0; j < nodes[i].responses.Length; j++)
			{
				Managers.EventManager.OnThrowFlag -= nodes[i].responses[j].CheckFlag;
			}
		}

		Managers.EventManager.OnDialogueEnd?.Invoke();
	}

	private void Start()
	{

		checkPointNode = nodes[currNode].checkPointConnection;
		exitNode = nodes[currNode].exitConnection;

		//move to start once text size is decided
		//textSize = textSize * Screen.width / 1920;

	}

	private void Update()
	{
		//-1 should be a signal to end the current discussion
		if (currNode != -1 && enabled)
		{
			//this can probs be moved out of update tbh
			DisplayNode(nodes[currNode]);
		}
		else
		{
			Debug.Log(ID + " is done talking");
			gameObject.SetActive(false);

			//A temporary comment. This exit node should be -1, but I don't wanna set them all by hand right now
			//currNode = exitNode;
		}
	}

	void DisplayNode(DialogueNode node)
	{
		checkPointNode = nodes[currNode].checkPointConnection;
		exitNode = nodes[currNode].exitConnection;

		promptPanel.text = nodes[currNode].prompt;

		if (nodes[currNode].speakerPic != null)
		{
			nodes[currNode].speakerPic.gameObject.SetActive(true);
		}

		int i = 0;

		if (nodes[currNode].responses.Length > 0) //if we have responses
		{
			for (; i < nodes[currNode].responses.Length; i++)
			{
				if (!nodes[currNode].responses[i].hidden)
				{
					responseButtons[i].gameObject.SetActive(true);
					responseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = nodes[currNode].responses[i].response;
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
		if (nodes[currNode].speakerPic != null)
		{
			nodes[currNode].speakerPic.gameObject.SetActive(false);
		}

		if (nodes[currNode].responses.Length == 0)
		{
			//-2 should be changed. it represents a "no connection" value for reference
			//-1 means "end of convo"
			if (nodes[currNode].connection == 0 && nodes[currNode].connection != -1) 
			{
				currNode++;
			}
			else
			{
				currNode = nodes[currNode].connection;
			}
		
		}
	}

	public void SelectResponse(int responseNode)
	{
		if (nodes[currNode].speakerPic != null)
		{
			nodes[currNode].speakerPic.gameObject.SetActive(false);
		}

		if (nodes[currNode].responses[responseNode].throwFlag != "")
		{
			Managers.EventManager.OnThrowFlag?.Invoke(nodes[currNode].responses[responseNode].throwFlag);
		}

		SetCurrNode(nodes[currNode].responses[responseNode].connection);
	}

	#endregion

	public void EnableCurrNode()
	{
		//currNode = 0;
		currNode = PlayerPrefs.GetInt(Managers.GameStateManager.Instance.CurrentTime + gameObject.name, 0);

		if (currNode != -1)
		{
			gameObject.SetActive(true);
		}
	}

	public void SetCurrNode(int newNode)
	{
		currNode = newNode;
	}

	private void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			if (this.enabled == true)
			{
				//the interrupt node
				int interruptNode = nodes[currNode].interruptConnection;
				int checkpoint = nodes[currNode].checkPointConnection;
				//set the interrupt node to go to the checkpoint node
				//This means interrupts can't ramble
				nodes[interruptNode].connection = checkPointNode;
				nodes[interruptNode].checkPointConnection = checkpoint;

				//now go to the interruptNode
				currNode = interruptNode;
				this.enabled = false;
			}
		}
	}

	#region Window Functions

	public void SetStringVal(string val)
	{
		nodes[0].prompt = val;
	}

	#endregion

}
