using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : ManagerBase<TaskManager>
{
	[InspectorButton("AddTaskTest")]
	public bool AddTaskButton = false;

	[InspectorButton("RemoveTaskTest")]
	public bool RemoveTaskButton = false;

	public List<Task> taskList;
	//public List<TaskUI> taskUIList; //create a taskUI class //why did I need this? if you figure it out, replace var of same name
	
        //Hey Karim I commented this out since it was giving a warning lolz. Uncomment it when you wanna use it
        ///[SerializeField] GameObject taskContainer = null; //where to get my list of tasks, as well as where to put them
	[SerializeField] GameObject taskUIReference = null;
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
			taskList[i].InitTask();
		}

		//EventAnnouncer.OnThrowFlag += UpdateTasks;
		UpdateTaskDisplay();
	}

	public void AddTask(Task newTask)
	{
		if (!taskList.Contains(newTask))
		{
			Debug.Log(newTask.name);
			taskList.Add(newTask);
		}
	}
#if UNITY_EDITOR
	public void AddTaskTest() //TEST FUNCTION
	{
		taskList.Add(editorAllTasks[0]);
		UpdateTaskDisplay();
		UpdateTasks();
	}
#endif
	public void RemoveTask(Task oldTask)
	{
		if (taskList.Contains(oldTask))
		{
			Debug.Log(oldTask.name);
			taskList.Remove(oldTask);
		}
	}
#if UNITY_EDITOR
	public void RemoveTaskTest() //TEST FUNCTION
	{
		if (taskList.Count != 0)
		{
			taskList.Remove(taskList[taskList.Count - 1]);
			UpdateTaskDisplay();
		}
	}
#endif

	/// <summary>
	/// Updates the tasks recursivly, as well as the UI at the same time
	/// </summary>
	public void UpdateTasks()
	{
		allTasksComplete = true;

		for (int i = 0; i < taskList.Count; i++)
		{
			taskList[i].UpdateTask();

			if (!taskList[i].isTaskComplete)
			{
				allTasksComplete = false;
			}
		}
	}

	void UpdateTaskDisplay()
	{
		foreach (Task task in taskList)
		{
			task.CreateUIElement(this.gameObject, taskUIReference);
		}
	}
}
