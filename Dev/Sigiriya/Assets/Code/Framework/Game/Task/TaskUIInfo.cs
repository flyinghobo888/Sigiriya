using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUIInfo : MonoBehaviour
{
	[SerializeField] public Task task;
	[SerializeField] private Image checkbox;
	[SerializeField] private Text taskDescription;
	[SerializeField] public List<Text> subTasks; //I need a way to display the subtasks as well. This might not cut it

	public void UpdateTaskUI()
	{
		taskDescription.text = task.description;
		
		if (task.isTaskComplete == true)
		{
			checkbox.color = Color.black;
		}
	}

}
