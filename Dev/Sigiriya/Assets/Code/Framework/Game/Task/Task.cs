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
	//Task name, and quick description
	public string taskName;
	public string description;

	//Either a list of tasks, OR flags to complete this task
	public List<Task> subTasks;
	public List<FlagBank.Flags> requirementFlags;

	//if we have flags, we create a dictionary to hold them
	public Dictionary<FlagBank.Flags, bool> requirementStatus;
	public bool isTaskComplete;

	[SerializeField] GameObject myUI;
	bool isHidden; //needs to be implemented
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
	//update the bool value of the passed in key if it exists
	public void UpdateTask(FlagBank.Flags flag)
	{
		if (requirementFlags.Count > 0 && requirementStatus.ContainsKey(flag))
		{
			Debug.Log("Task is Done! I think");
			requirementStatus[flag] = true;

			if (requirementStatus.ContainsValue(false))
			{
				isTaskComplete = false;
			}
			else
			{
				Debug.Log("Task " + taskName + " is Done! I think");
				isTaskComplete = true;
			}
		}
		else if (requirementFlags.Count == 0)
		{
			isTaskComplete = true;

			foreach (Task task in subTasks)
			{
				task.UpdateTask(flag);

				if (!task.isTaskComplete)
				{
					isTaskComplete = false;
				}
			}
		}
	}

	public void CreateUIElement(GameObject parentObject, GameObject uiReference)
	{
		//instantiate a UI reference, and child it the the parent. then call function for 
		//all tasks this holds, with itself as the parent

		if (myUI == null)
		{
			myUI = Instantiate(uiReference);
			myUI.transform.parent = parentObject.transform;
			myUI.GetComponent<TaskUIInfo>().task = this;
		}
		foreach (Task task in subTasks)
		{
			task.CreateUIElement(myUI , uiReference);
		}

		//create the prefab and set to the member var
		//if it exists, don't create it, and instead update it, or delete it
	}
	//check if all subtasks are complete
//	public void CheckTaskRequirements()
//	{
//		if (requirementFlags.Count > 0)
//		{
//			if (requirementStatus.ContainsValue(false))
//			{
//				isTaskComplete = false;
//
//				return;
//			}
//			Debug.Log("Task " + taskName + " is Done! I think");
//			isTaskComplete = true;
//		}
//		else
//		{
//			foreach (Task task in subTasks)
//			{
//				task.CheckTaskRequirements();
//				if (!task.isTaskComplete)
//				{
//					isTaskComplete = false;
//					return;
//				}
//			}
//			isTaskComplete = true;
//			return;
//		}
//	}	


	//public void InitAllSubTasks()
	//{
	//	if (subTasks == null)
	//	{
	//		Debug.Log("There are no subtasks!");
	//		return;
	//	}
	//
	//	for (int i = 0; i < subTasks.Count; i++)
	//	{
	//		subTasks[i].InitSubTask();
	//	}
	//}
	//
	//public void UpdateSubTasks(FlagBank.Flags flag)
	//{
	//	for (int i = 0; i < subTasks.Count; i++)
	//	{
	//		subTasks[i].UpdateSubTask(flag);
	//	}
	//
	//	CheckTaskRequirements();
	//}
	//
	////check if all subtasks are complete
	//public void CheckTaskRequirements()
	//{
	//	for (int i = 0; i < subTasks.Count; i++)
	//	{
	//		if (subTasks[i].requirementStatus.ContainsValue(false))
	//		{
	//			isTaskComplete = false;
	//
	//			return;
	//		}
	//	}
	//	Debug.Log("Task " + taskName + " is Done! I think");
	//	isTaskComplete = true;
	//}
}
