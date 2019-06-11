using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentEventBank : MonoBehaviour
{
    private static List<string> eventFlags;

    //private static int peopleMorning = 5;
    //private static int peopleNoon = 7;
    //private static int peopleNight = 5;
    //private static int peopleTalked = 0;

    private void Awake()
    {
		if (eventFlags == null)
		{
			eventFlags = new List<string>();
		}
		EventAnnouncer.OnThrowFlag += AddEvent;
        EventAnnouncer.OnDialogueEnd += IncrementPeopleTalkedTo;
        PlayerPrefs.DeleteAll();
    }

    public static void FireAllEvents()
    {
		if (eventFlags == null)
		{
			eventFlags = new List<string>();
		}
        for (int i = 0; i < eventFlags.Count; i++)
        {
            Debug.Log(eventFlags[i]);
            EventAnnouncer.OnThrowFlag?.Invoke(eventFlags[i]);
        }
    }

    private static void AddEvent(string flag)
    {
        if (!eventFlags.Contains(flag))
        {
            eventFlags.Add(flag);
        }
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
