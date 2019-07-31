using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : ManagerBase<TaskManager>
{
	[InspectorButton("AddTaskAgain")]
	public bool AddTaskButton = false;

	[InspectorButton("RemoveTaskAgain")]
	public bool RemoveTaskButton = false;

	public List<Task> taskList;
	//public List<TaskUI> taskUIList; //create a taskUI class //why did I need this? if you figure it out, replace var of same name
	[SerializeField] GameObject taskContainer; //where to get my list of tasks, as well as where to put them
	[SerializeField] GameObject taskUIReference;
	[SerializeField] private List<GameObject> taskUIList;

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
		if (taskUIList == null)
		{
			taskUIList = new List<GameObject>();
		}

		for (int i = 0; i < taskList.Count; i++)
		{
			taskList[i].InitAllSubTasks();
		}

		EventAnnouncer.OnThrowFlag += UpdateTasks;
		UpdateTaskDisplay();
	}

	public void AddTask(Task newTask)
	{
		if (!taskList.Contains(newTask))
		{
			Debug.Log(newTask.taskName);
			taskList.Add(newTask);
		}
	}
	public void AddTaskAgain()
	{
		taskList.Add(editorAllTasks[0]);
		UpdateTaskDisplay();
	}

	public void RemoveTask(Task oldTask)
	{
		if (taskList.Contains(oldTask))
		{
			Debug.Log(oldTask.taskName);
			taskList.Remove(oldTask);
		}
	}
	public void RemoveTaskAgain()
	{
		if (taskList.Count != 0)
		{
			taskList.Remove(taskList[taskList.Count - 1]);
			UpdateTaskDisplay();
		}
	}

	public void UpdateTasks(FlagBank.Flags flag)
	{
		for (int i = 0; i < taskList.Count; i++)
		{
			taskList[i].UpdateSubTasks(flag);
		}

		CheckTasks();
		UpdateTaskDisplay();
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

	void UpdateTaskDisplay()
	{
		int numChild = taskContainer.transform.childCount;
		while (taskList.Count > numChild)
		{
			GameObject taskUI = Instantiate(taskUIReference) as GameObject;

			taskUI.transform.SetParent(taskContainer.transform);
			taskUIList.Add(taskUI);

			numChild++;
		}
		if (taskList.Count < numChild)
		{
			while (taskUIList.Count > taskList.Count)
			{
				GameObject taskUI = taskUIList[taskUIList.Count - 1];
				taskUIList.Remove(taskUI);

				Destroy(taskUI);
			}
		}

		for (int i = 0; i < taskUIList.Count; i++)
		{
			TaskUIInfo taskInfo = taskUIList[i].GetComponent<TaskUIInfo>();

			taskInfo.task = taskList[i];
			taskInfo.UpdateTaskUI();
		}
	}
}
