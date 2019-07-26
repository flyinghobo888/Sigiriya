using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : ManagerBase<TaskManager>
{
	public List<Task> taskList;
	public bool allTasksComplete = false;

#if UNITY_EDITOR
	public List<Task> editorAllTasks;
#endif

	public void Awake()
	{
#if UNITY_EDITOR
		foreach (Task task in editorAllTasks)
		{
			task.isTaskComplete = false;
		}
		allTasksComplete = false;
#endif


		if (taskList == null)
		{
			taskList = new List<Task>();
		}

		for (int i = 0; i < taskList.Count; i++)
		{
			taskList[i].InitAllSubTasks();
		}

		EventAnnouncer.OnThrowFlag += UpdateTasks;
	}

	public void AddTask(Task newTask)
	{
		if (!taskList.Contains(newTask))
		{
			Debug.Log(newTask.taskName);
			taskList.Add(newTask);
		}
	}

	public void RemoveTask(Task oldTask)
	{
		if (taskList.Contains(oldTask))
		{
			Debug.Log(oldTask.taskName);
			taskList.Remove(oldTask);
		}
	}

	public void UpdateTasks(FlagBank.Flags flag)
	{
		for (int i = 0; i < taskList.Count; i++)
		{
			taskList[i].UpdateSubTasks(flag);
		}

		CheckTasks();
	}

	public void CheckTasks()
	{
		if (taskList.Count <= 0)
		{
			return;
		}

		allTasksComplete = true;
		for (int i = 0; i < taskList.Count; i++)
		{
			taskList[i].CheckTaskRequirements();
			if (!taskList[i].isTaskComplete)
			{
				allTasksComplete = false;
			}
		}
	}
}
