using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryManager : ManagerBase<MemoryManager>
{
	//public GameObject memoryUIReference;

	public List<Memory> recordedMemories;
	public List<Memory> unrecordedMemories;

	//public Transform memoryContainer;

	[InspectorButton("DisplayMemories")]
	public bool Display = false;

	// Start is called before the first frame update
	void Start()
    {

    }

	/// <summary>
	/// Called whenever memory UI is opened
	/// </summary>
	public void DisplayMemories()
	{
        //TODO: can be moved to whatever functions decides the new memory instead of a loop
        //while (recordedMemories.Count > memoryContainer.childCount)
        //{
        //	GameObject newMem = Instantiate(memoryUIReference) as GameObject;

        //	newMem.GetComponentInChildren<Image>().sprite = recordedMemories[0].memoryImage;
        //	newMem.GetComponentInChildren<Text>().text = recordedMemories[0].memoryText;

        //	newMem.transform.SetParent(memoryContainer);
        //}

        ScrollBoardController.Instance.ResetMemoryUIList();
        foreach (Memory memory in recordedMemories)
        {
            ScrollBoardController.Instance.AddUIItem(memory);
        }
	}

	/// <summary>
	/// Called when a <see cref="MemoryNode"/> is evaluated
	/// </summary>
	/// <param name="memory"></param>
	public void AddMemory(Memory memory)
	{
		if (!unrecordedMemories.Contains(memory))
		{
			unrecordedMemories.Add(memory);
		}
	}

	/// <summary>
	/// Called at some point during the EndOfDay
	/// </summary>
	/// <param name="memory"></param>
	public void MoveMemoryToRecorded(Memory memory)
	{
		if (unrecordedMemories.Contains(memory))
		{
			//unrecordedMemories.Remove(memory);
			recordedMemories.Add(memory);
		}

		//TODO: clear unrecorded??
		unrecordedMemories.Clear();
	}
}
