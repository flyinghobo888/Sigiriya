using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TaskData", menuName = "Task")]
public class Task : ScriptableObject
{
	public string taskName;
	public List<FlagBank.Flags> requirements;
	public bool isTaskComplete;

	public void CheckTaskRequirements()
	{
		if (PersistentEventBank.ContainsFlag(requirements[0]))
		{
			Debug.Log("Task is Done! I think");
			isTaskComplete = true;
		}
	}
}
