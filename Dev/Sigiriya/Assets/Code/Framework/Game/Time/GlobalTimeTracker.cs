using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
//The Global Time Tracker is responsible for tracking the time through out the game.
public class GlobalTimeTracker : ManagerBase<GlobalTimeTracker>
{
    //For now, just change the time every 5 seconds
    private float currentTime;
    private EnumTime currentTimeOfDay;

    private void Start()
    {
        currentTimeOfDay = EnumTime.MORNING;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 1.0f)
        {
            currentTime = 0.0f;
            currentTimeOfDay = (EnumTime)(((int)currentTimeOfDay + 1) % (int)EnumTime.SIZE);
            //Debug.Log("Time of Day: " + currentTimeOfDay);
        }
    }
}

//In the future, we can track actual hours, but for now only track with an enum
public enum EnumTime
{
    SUNRISE,
    MORNING,
    MIDDAY,
    EVENING,
    NIGHT,
    SIZE
}
