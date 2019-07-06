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

    [Header ("UI Fields")]
    [SerializeField] private TextMeshProUGUI nameBox = null;
    [SerializeField] private TextMeshProUGUI promptPanel = null;
    [SerializeField] private Button [] responseButtons = null;
    [SerializeField] private Button continueButton = null;
    [SerializeField] private List<Image> speakerImages; //basically just a holder of images to display 

    [Header ("Dialogue")]
    //[SerializeField] public List<DialogueNode> nodes;
    //[SerializeField] BaseNode currNode;
    [SerializeField] PromptNode pNode;
    private bool isInit = false;

    private string ID;
    private BaseNode checkPointNode = null;
    private BaseNode exitNode = null;

    private float talkTimer = 0;

    private Vector3 worldToScreenPos;

    private void Awake()
    {
        if (!isInit)
        {
            Init ();
        }
    }

    private void Init()
    {
        ID = this.name;
        //EventManager.StartListening(ID + "_enable", EnableCurrNode);
        dialogueGraph.Restart ();
        //currNode = dialogueGraph.current;

        //TODO: maybe make this a loop? this way I can string different nodes instead of needing a prompt after things. 
        //if i do make loop, i need to make sure it checks if the node is null, in case i reach the end before a prompt
        //OR just add an empty prompt and link through that
        if (dialogueGraph.current.GetType () != typeof (PromptNode))
        {
            if (dialogueGraph.current.GetType () == typeof (BranchNode))
            {
                BranchNode bNode = dialogueGraph.current as BranchNode;

                dialogueGraph.current = bNode.GetOutputNode () as PromptNode;
            }
        }

        pNode = dialogueGraph.current as PromptNode;
        //gameObject.SetActive(false);
        isInit = true;
    }

    private void OnEnable()
    {
        //EventManager.StartListening("E_down", ContinueDialogue);
        //EventManager.FireEvent("MENU_open");

        for (int i = 0 ; i < dialogueGraph.nodes.Count ; i++)
        {
            //Debug.Log(dialogueGraph.nodes[i].GetType());

            if (dialogueGraph.nodes [i].GetType () == typeof (PromptNode))
            {
                PromptNode node = dialogueGraph.nodes [i] as PromptNode;

                if (node.responses != null)
                {
                    for (int j = 0 ; j < node.responses.Count ; j++)
                    {
                        //EventAnnouncer.OnThrowFlag += node.responses[j].CheckFlag;
                    }
                }
            }
        }

        PersistentEventBank.FireAllEvents ();
    }
    private void OnDisable()
    {
        //EventManager.StopListening("E_down", ContinueDialogue);
        //EventManager.FireEvent("MENU_close");

        //TODO: THIS IS FOR OLD PROTOTYPE PLZ FIX
        ///PlayerPrefs.SetInt(Managers.GameStateManager.Instance.CurrentTime + gameObject.name, dialogueGraph.current);

        for (int i = 0 ; i < dialogueGraph.nodes.Count ; i++)
        {
            if (dialogueGraph.nodes [i].GetType () == typeof (PromptNode))
            {
                PromptNode node = dialogueGraph.nodes [i] as PromptNode;

                if (node.responses != null)
                {
                    for (int j = 0 ; j < node.responses.Count ; j++)
                    {
                        //EventAnnouncer.OnThrowFlag -= node.GetAnswerConnection(j).CheckFlag;
                    }
                }
            }
        }

        EventAnnouncer.OnDialogueEnd?.Invoke ();
    }

    private void Start()
    {
        //Maybe set extraConnections again here? I dunno.

        //move to start once text size is decided
        //textSize = textSize * Screen.width / 1920;

    }

    private void Update()
    {
        //TODO: maybe make this a loop? this way I can string different nodes instead of needing a prompt after things. 
        //if i do make loop, i need to make sure it checks if the node is null, in case i reach the end before a prompt
        //OR just add an empty prompt and link through that
        if (dialogueGraph.current.GetType () != typeof (PromptNode))
        {
            if (dialogueGraph.current.GetType () == typeof (BranchNode))
            {
                BranchNode bNode = dialogueGraph.current as BranchNode;

                dialogueGraph.current = bNode.GetOutputNode () as PromptNode;
            }
        }

        pNode = dialogueGraph.current as PromptNode;

        //-1 should be a signal to end the current discussion
        if (dialogueGraph.current != null && enabled)
        {
            //this can probs be moved out of update tbh
            DisplayNode (pNode);
        }
        else
        {
            Debug.Log (ID + " is done talking");
            gameObject.SetActive (false);

            dialogueGraph.current = exitNode as PromptNode;
        }

        talkTimer += Time.deltaTime;

        if (pNode.time == 0)
        {
            //talkTimer = 0;
            Debug.Log ("Node is missing a time!");
        }
        if (talkTimer > pNode.time)
        {
            if (dialogueGraph.current.GetNextNode () != null && enabled)
            {
                ContinueDialogue ();
            }
            else
            {
                Debug.Log (ID + " is done talking");
                gameObject.SetActive (false);

                dialogueGraph.current = exitNode;
            }

            talkTimer = 0;
        }
    }

    void DisplayNode(PromptNode node)
    {
        checkPointNode = node.GetConnectedNode ("checkpointConnection");
        exitNode = node.GetConnectedNode ("exitConnection");

        //nameBox.text = node.speaker==null ? "Player" : node.speaker.characterName;
        promptPanel.text = node.prompt;

        /*
		 * if the speakerImage.sprite exists in the nodes.speakerPic list, go nuts. else, other way around
		 */
        Sprite speakerPic = node.GetSprite (true);
        if (speakerPic != null)
        {
            speakerImages [0].sprite = speakerPic;
            speakerImages [0].gameObject.SetActive (true);
        }
        int i = 0;

        if (node.responses != null) //if we have responses
        {
            for (; i < node.responses.Count ; i++)
            {
                if (!node.GetAnswerConnection (i).getHidden ())
                {
                    responseButtons [i].gameObject.SetActive (true);
                    responseButtons [i].GetComponentInChildren<TextMeshProUGUI> ().text = node.GetAnswerConnection (i).text;
                }
                else
                {
                    responseButtons [i].gameObject.SetActive (false);
                }
            }
            continueButton.gameObject.SetActive (false);
        }
        else
        {
            //TODO: Timed dialogue continues now, so remove this gracefully after the continue button becomes obsolete
            continueButton.gameObject.SetActive (true);
        }

        for (; i < responseButtons.Length ; i++)
        {
            responseButtons [i].gameObject.SetActive (false);
        }
    }

    #region Button Commands
    //exposed to the UI continue button
    public void ContinueDialogue()
    {
        Sprite speakerPic = pNode.GetSprite (false);
        if (speakerPic != null)
        {
            speakerImages [0].sprite = speakerPic;
            speakerImages [0].gameObject.SetActive (true);
        }

        dialogueGraph.current = dialogueGraph.current.GetNextNode ();

        talkTimer = 0;
    }

    public void SelectResponse(int responseNode)
    {
        //TODO: fix this for the image swap from the character system
        //if (pNode.speakerPic != null)
        //if (pNode.speakerPic == speakerImages[0].sprite)
        //{
        //	speakerImages[0].gameObject.SetActive(true);
        //	speakerImages[1].gameObject.SetActive(false);
        //}
        //else if (pNode.speakerPic == speakerImages[1].sprite)
        //{
        //	speakerImages[1].gameObject.SetActive(true);
        //	speakerImages[0].gameObject.SetActive(false);
        //}

        Sprite speakerPic = pNode.GetSprite (false);
        if (speakerPic != null)
        {
            speakerImages [0].sprite = speakerPic;
            speakerImages [0].gameObject.SetActive (true);
        }

        if (pNode.GetAnswerConnection (responseNode).throwFlag != FlagBank.Flags.NONE)
        {
            EventAnnouncer.OnThrowFlag?.Invoke (pNode.GetAnswerConnection (responseNode).throwFlag);
        }

        dialogueGraph.current = pNode.GetAnswerConnection (responseNode).GetNextNode ();

        talkTimer = 0;
    }

    #endregion

    public void EnableCurrNode()
    {
        //dialogueGraph.current = checkPointNode;
        //dialogueGraph.current = exitNode;

        //TODO: THIS IS FOR OLD PROTOTYPE PLZ FIX
        ///dialogueGraph.current = PlayerPrefs.GetInt(Managers.GameStateManager.Instance.CurrentTime + gameObject.name, 0);

        if (!isInit)
        {
            Init ();
        }
        if (dialogueGraph.current != null)
        {
            //TODO: maybe make this a loop? this way I can string different nodes instead of needing a prompt after things. 
            //if i do make loop, i need to make sure it checks if the node is null, in case i reach the end before a prompt
            //OR just add an empty prompt and link through that
            if (dialogueGraph.current.GetType () != typeof (PromptNode))
            {
                if (dialogueGraph.current.GetType () == typeof (BranchNode))
                {
                    BranchNode bNode = dialogueGraph.current as BranchNode;

                    dialogueGraph.current = bNode.GetOutputNode () as PromptNode;
                }
            }

            pNode = dialogueGraph.current as PromptNode;

            DisplayNode (pNode);
            gameObject.SetActive (true);
        }
    }

    public void SetCurrNode(int newNode)
    {
        dialogueGraph.current = dialogueGraph.nodes [newNode] as BaseNode;// as PromptNode;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            if (this.enabled == true)
            {
                //the interrupt node
                BaseNode interruptNode = pNode.GetConnectedNode ("interruptConnection");
                BaseNode checkpoint = pNode.GetConnectedNode ("checkpointConnection");

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
}
