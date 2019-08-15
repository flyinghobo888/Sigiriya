using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:i have half a mind to make tasks "recursive"
/*I think I will
	So:
	Tasks will either refer to a flag to decide if they are complete, of if all their tasks are complete
	What this means:
	-Each task either has a list of tasks, or list of flags
	-if it has a list of tasks, those tasks will refer to a list of tasks or a list of flags, etc.
	-the task manager is just used to get a quick answer to the top level of tasks
	-each task should be responsible for it's ui somehow. or something like that
	 */



[CreateAssetMenu(fileName = "TaskData", menuName = "Task")]
public class Task : ScriptableObject
{
	//quick description
	public string description;

	//Either a list of tasks, OR flags to complete this task
	public List<Task> subTasks;
	public List<FlagBank.Flags> requirementFlags;

	//if we have flags, we create a dictionary to hold them
	public Dictionary<FlagBank.Flags, bool> requirementStatus;
	public bool isTaskComplete;

	[SerializeField] GameObject myUI;
	bool isHidden; //TODO: needs to be implemented
	//if a certain flag is hit, this can be revealed to the player

	public void InitTask()
	{
		//if we have flags, create the flag checking thing
		if (requirementFlags.Count > 0)
		{
			if (requirementStatus == null)
			{
				requirementStatus = new Dictionary<FlagBank.Flags, bool>();
			}

			//fill out the dictionary with its flags and a default false
			for (int i = 0; requirementStatus.Count < requirementFlags.Count; i++)
			{
				bool activeVal = false;
				requirementStatus.Add(requirementFlags[i], activeVal);
			}

			//correct any flag values if they are actually active
			foreach (KeyValuePair<FlagBank.Flags, bool> flag in requirementStatus)
			{
				if (PersistentEventBank.ContainsFlag(flag.Key))
				{
					requirementStatus[flag.Key] = true;
					Debug.Log(flag.Key);
				}
			}
		}
		else
		{
			foreach (Task task in subTasks)
			{
				task.InitTask();
			}
		}
	}
	//update the bool value of the passed in key if it exists //why am I passing in a flag?
	public void UpdateTask()
	{
		if (requirementFlags.Count > 0)
		{
			foreach (FlagBank.Flags flag in requirementFlags)
			{
				if (PersistentEventBank.ContainsFlag(flag))
				{
					requirementStatus[flag] = true;
				}
			}


			if (requirementStatus.ContainsValue(false))
			{
				isTaskComplete = false;
			}
			else
			{
				Debug.Log("Task " + this.name + " is Done! I think");
				isTaskComplete = true;
			}
		}
		else if (requirementFlags.Count == 0)
		{
			isTaskComplete = true;

			foreach (Task task in subTasks)
			{
				task.UpdateTask();

				if (!task.isTaskComplete)
				{
					isTaskComplete = false;
				}
			}
		}

		TaskUIInfo taskInfo = myUI.GetComponent<TaskUIInfo>();
		taskInfo.UpdateTaskUI();
	}

	public void CreateUIElement(GameObject parentObject, GameObject uiReference)
	{
		//instantiate a UI reference, and child it the the parent. then call function for 
		//all tasks this holds, with itself as the parent

		//TODO: if I am a parent task, create a ui thing. If I am a leaf, instantiate nothing, return
		//this way, the parent can create the prefab for the multitask UI object, and add data from all 3 to it
		if (myUI == null)
		{
			myUI = Instantiate(uiReference);
			myUI.transform.SetParent(parentObject.transform);
			myUI.GetComponent<TaskUIInfo>().task = this;
		}
		foreach (Task task in subTasks)
		{
			task.CreateUIElement(parentObject, uiReference);
		}

	}
}
