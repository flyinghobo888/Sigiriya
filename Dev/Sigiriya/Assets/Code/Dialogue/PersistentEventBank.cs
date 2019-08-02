using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentEventBank : MonoBehaviour
{
	//TODO: might want to move all of this to the FlagBank
	private static List<FlagBank.Flags> eventFlags;

	private void Awake()
    {
		if (eventFlags == null)
		{
			eventFlags = new List<FlagBank.Flags>();
		}
		EventAnnouncer.OnThrowFlag += AddEvent;
        EventAnnouncer.OnDialogueEnd += IncrementPeopleTalkedTo;
        PlayerPrefs.DeleteAll();
    }

    public static void FireAllEvents()
    {
		if (eventFlags == null)
		{
			eventFlags = new List<FlagBank.Flags>();
		}
        for (int i = 0; i < eventFlags.Count; i++)
        {
            Debug.Log(eventFlags[i]);
            EventAnnouncer.OnThrowFlag?.Invoke(eventFlags[i]);
        }
    }

	public static bool ContainsFlag(FlagBank.Flags flag)
	{
		if (eventFlags.Contains(flag))
		{
			return true;
		}

		return false;
	}

    private static void AddEvent(FlagBank.Flags flag)
    {
        if (!eventFlags.Contains(flag))
        {
            eventFlags.Add(flag);
        }

		//Check task events whenever an event is added
		//TaskManager.Instance.CheckTasks();
    }

    private static void IncrementPeopleTalkedTo()
    {
        //if (Managers.GameStateManager.Instance.CurrentTime == EnumTime.MORNING)
        //{
        //    if (peopleTalked < peopleMorning - 1)
        //    {
        //        peopleTalked++;
        //    }
        //    else
        //    {
        //        peopleTalked = 0;
        //        Managers.GameStateManager.Instance.SetTime(EnumTime.NOON);
        //        Managers.GameStateManager.Instance.GoToNextScene(EnumScene.HOME);
        //    }
        //}
        //else if (Managers.GameStateManager.Instance.CurrentTime == EnumTime.NOON)
        //{
        //    if (peopleTalked < peopleNoon - 1)
        //    {
        //        peopleTalked++;
        //    }
        //    else
        //    {
        //        peopleTalked = 0;
        //        Managers.GameStateManager.Instance.SetTime(EnumTime.NIGHT);
        //        Managers.GameStateManager.Instance.GoToNextScene(EnumScene.HOME);
        //    }

        //}
        //else if (Managers.GameStateManager.Instance.CurrentTime == EnumTime.NIGHT)
        //{
        //    if (peopleTalked < peopleNight - 1)
        //    {
        //        peopleTalked++;
        //    }
        //    else
        //    {
        //        peopleTalked = 0;
        //        Debug.Log("GAME OVER");
        //        Managers.GameStateManager.Instance.SetTime(EnumTime.MORNING);
        //        Managers.GameStateManager.Instance.GoToNextScene((EnumScene)19);
        //    }
        //}
    }
}
