using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : ManagerBase<MemoryManager>
{
	public List<Memory> recordedMemories;
	public List<Memory> unrecordedMemories;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void AddMemory(Memory memory)
	{
		if (!unrecordedMemories.Contains(memory))
		unrecordedMemories.Add(memory);
	}

	public void MoveMemoryToRecorded(Memory memory)
	{
		if (unrecordedMemories.Contains(memory))
		{
			unrecordedMemories.Remove(memory);
			recordedMemories.Add(memory);
		}

		//TODO: clear unrecorded??
	}
}
