using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//The Global Time Tracker is responsible for tracking the time through out the game.
public class GlobalTimeTracker : ManagerBase<GlobalTimeTracker>
{
    //For now, just change the time every few seconds
    private float totalTime = 5.0f;
    public float TotalTimeInverse { get; private set; }
    public float CurrentTime { get; private set; } = 0.0f;

    //How far into the cycle the time is.
    public float TimeAlpha { get; private set; } = 0.0f;

    public EnumTime CurrentTimeOfDay { get; private set; }

    private int currentIndex = 0;

    private void Start()
    {
        TotalTimeInverse = 1.0f / totalTime;
        CurrentTimeOfDay = EnumTime.SUNRISE;
    }

    private void Update()
    {
        CurrentTime += Time.deltaTime;
        TimeAlpha = CurrentTime * TotalTimeInverse;

        if (CurrentTime >= totalTime)
        {
            currentIndex = (((int)CurrentTimeOfDay + 1) % (int)EnumTime.SIZE);
            CurrentTime = 0.0f;
            CurrentTimeOfDay = (EnumTime)(currentIndex);
            TimeAlpha = 0.0f;
            EventAnnouncer.OnTimeChanged(CurrentTimeOfDay);
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
