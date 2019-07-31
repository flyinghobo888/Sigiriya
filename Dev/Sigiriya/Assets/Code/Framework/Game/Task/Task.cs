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
	[System.Serializable]
	public class SubTask
	{
		public string taskName;
		public string description;
		public List<FlagBank.Flags> requirementFlags;
		public Dictionary<FlagBank.Flags, bool> requirementStatus;

		bool isHidden; //needs to be implemented
		//if a certain flag is hit, this can be revealed to the player

		public void InitSubTask()
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

		//update the bool value of the passed in key if it exists
		public void UpdateSubTask(FlagBank.Flags flag)
		{
			if (requirementStatus.ContainsKey(flag))
			{
				Debug.Log("Task is Done! I think");
				requirementStatus[flag] = true;
			}
		}
	}

	public string taskName;
	public string description;
	public List<SubTask> subTasks;	
	public bool isTaskComplete;

	public void InitAllSubTasks()
	{
		if (subTasks == null)
		{
			Debug.Log("There are no subtasks!");
			return;
		}

		for (int i = 0; i < subTasks.Count; i++)
		{
			subTasks[i].InitSubTask();
		}
	}

	public void UpdateSubTasks(FlagBank.Flags flag)
	{
		for (int i = 0; i < subTasks.Count; i++)
		{
			subTasks[i].UpdateSubTask(flag);
		}

		CheckTaskRequirements();
	}

	//check if all subtasks are complete
	public void CheckTaskRequirements()
	{
		for (int i = 0; i < subTasks.Count; i++)
		{
			if (subTasks[i].requirementStatus.ContainsValue(false))
			{
				Debug.Log("Task " + taskName + " is Done! I think");
				isTaskComplete = false;

				return;
			}
		}

		isTaskComplete = true;
	}
}
