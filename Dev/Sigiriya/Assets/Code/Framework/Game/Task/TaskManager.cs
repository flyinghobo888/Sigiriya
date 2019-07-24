using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : ManagerBase<TaskManager>
{
	public List<Task> taskList;

	public void Awake()
	{
		if (taskList == null)
		{
			taskList = new List<Task>();
		}
	}

	public void AddTask(Task newTask)
	{
		if (!taskList.Contains(newTask))
		{
			Debug.Log(newTask.taskName);
			taskList.Add(newTask);
		}
	}

	public void UpdateTasks()
	{
		if (taskList.Count <= 0)
		{
			return;
		}

		for (int i = 0; i < taskList.Count; i++)
		{
			taskList[i].CheckTaskRequirements();
		}
	}
}
