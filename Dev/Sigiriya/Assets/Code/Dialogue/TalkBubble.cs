using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBubble : MonoBehaviour
{
	[SerializeField] private GameObject talkbBubble;
	[SerializeField] private SimpleGraph myGraph;

	//TODO: instead of this script, rename it to ButtonInfo or something...
	//and have the button call functions from here 
	//Also, all graphs need to be initialized so that the bubbles work properly
	//as it stands, they are only init when you click on the button.
	//this needs to happen on startup, when graphs swap, etc.

    // Update is called once per frame
    void Update()
    {
		//see if you can pull this out of update
		CheckIfWantsToTalk();
    }

	void CheckIfWantsToTalk()
	{
		if (myGraph != null && talkbBubble != null)
		{
			if (!myGraph.isInit)
			{
				myGraph.Restart();
			}

			if (myGraph.current == null)
			{
				talkbBubble.SetActive(false);
			}
			else
			{
				talkbBubble.SetActive(true);
			}
		}
	}
}
