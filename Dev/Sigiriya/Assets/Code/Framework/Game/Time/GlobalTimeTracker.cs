using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//The Global Time Tracker is responsible for tracking the time through out the game.
public class GlobalTimeTracker : ManagerBase<GlobalTimeTracker>
{
    public SigiTime GlobalTime { get; private set; } = new SigiTime();
    private Dictionary<EnumDisplayTime, float> TimeOfDay = new Dictionary<EnumDisplayTime, float>();

    [Header("Hour at which time should switch")]
    public int SUNRISE_START = 6;
    public int MORNING_START = 7;
    public int MIDDAY_START = 11;
    public int EVENING_START = 16;
    public int NIGHT_START = 21;

    public EnumDisplayTime startTime = EnumDisplayTime.SUNRISE;
    public float DaySpeedMultiplier = 360.0f;

    //How far into the cycle the time is.
    public float TimeAlpha { get; private set; } = 0.0f;

    public EnumDisplayTime CurrentTimeOfDay { get; private set; }

    private void Start()
    {
        InitTimeOfDay();
        GlobalTime.Multiplier = DaySpeedMultiplier;

        if (TimeOfDay.TryGetValue(startTime, out float ticks))
        {
            GlobalTime.SetTickValue(ticks);
        }
    }

    private void Update()
    {
        GlobalTime.Tick();
        CalculateDisplayTime();
    }

    private void InitTimeOfDay()
    {
        TimeOfDay.Clear();
        TimeOfDay.Add(EnumDisplayTime.SUNRISE, SUNRISE_START * SigiTime.SECONDS * SigiTime.MINUTES);
        TimeOfDay.Add(EnumDisplayTime.MORNING, MORNING_START * SigiTime.SECONDS * SigiTime.MINUTES);
        TimeOfDay.Add(EnumDisplayTime.MIDDAY, MIDDAY_START * SigiTime.SECONDS * SigiTime.MINUTES);
        TimeOfDay.Add(EnumDisplayTime.EVENING, EVENING_START * SigiTime.SECONDS * SigiTime.MINUTES);
        TimeOfDay.Add(EnumDisplayTime.NIGHT, NIGHT_START * SigiTime.SECONDS * SigiTime.MINUTES);
    }

    private void CalculateDisplayTime()
    {
        CurrentTimeOfDay = GetTimeOfDay();
        EnumDisplayTime NextTimeOfDay = (EnumDisplayTime)(((int)CurrentTimeOfDay + 1) % (int)EnumDisplayTime.SIZE);
        TimeOfDay.TryGetValue(CurrentTimeOfDay, out float startTime);
        TimeOfDay.TryGetValue((EnumDisplayTime)((int)NextTimeOfDay), out float endTime);
        float currentTime = GlobalTime.Ticks;

        if (startTime > endTime)
        {
            endTime += SigiTime.TICKS;

            if (currentTime < startTime)
            {
                currentTime += SigiTime.TICKS;
            }
        }

        TimeAlpha = (currentTime - startTime) / (endTime - startTime);

        EventAnnouncer.OnTimeChanged(GlobalTime);
    }

    private EnumDisplayTime GetTimeOfDay()
    {
        if (GlobalTime.Hour < SUNRISE_START || GlobalTime.Hour >= NIGHT_START)
        {
            return EnumDisplayTime.NIGHT;
        }
        else if (GlobalTime.Hour < MORNING_START)
        {
            return EnumDisplayTime.SUNRISE;
        }
        else if (GlobalTime.Hour < MIDDAY_START)
        {
            return EnumDisplayTime.MORNING;
        }
        else if (GlobalTime.Hour < EVENING_START)
        {
            return EnumDisplayTime.MIDDAY;
        }
        else
        {
            return EnumDisplayTime.EVENING;
        }
    }

    public class SigiTime
    {
        public int Day { get; private set; }
        public int Hour { get; private set; }
        public int Minute { get; private set; }
        public int Second { get; private set; }

        public float Multiplier { get; set; } = 1.0f;

        private float totalTimeCount = 0.0f;
        private float tickCounter = 0.0f;
        public float Ticks = 0.0f;

        public const int HOURS = 24;
        public const int MINUTES = 60;
        public const int SECONDS = 60;
        public const float TICKS = HOURS * MINUTES * SECONDS;

        public SigiTime(int h = 0, int m = 0, int s = 0)
        {
            Hour = h;
            Minute = m;
            Second = s;
        }

        public void Tick()
        {
            totalTimeCount += Time.deltaTime * Multiplier;
            Ticks = totalTimeCount % TICKS;
            tickCounter = Ticks;

            Second = (int)Ticks;
            Second %= SECONDS;

            Minute = (int)(tickCounter /= SECONDS);
            Minute %= MINUTES;

            Hour = (int)(tickCounter /= MINUTES);
            Hour %= HOURS;

            Day = (int)(tickCounter /= HOURS);

            //Debug.Log("Time: " + Day + "d " + Hour + "h " + Minute + "m " + Second + "s");
        }

        public void SetTickValue(float tickValue)
        {
            totalTimeCount = tickValue;
        }
    }

    public enum EnumDisplayTime
    {
        SUNRISE,
        MORNING,
        MIDDAY,
        EVENING,
        NIGHT,
        SIZE
    }
}

public enum EnumTime
{
    MORNING = 1,
    MIDDAY,
    NIGHT = 4,
    SIZE = 5
}