using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//The Global Time Tracker is responsible for tracking the time through out the game.
public class GlobalTimeTracker : ManagerBase<GlobalTimeTracker>
{
    public SigiTime GlobalTime { get; private set; } = new SigiTime();
    private Dictionary<EnumDisplayTime, float> SwitchTimesList = new Dictionary<EnumDisplayTime, float>();

    [Header("Hour at which time should switch.")]
    public int SUNRISE_START = 6;
    public int MORNING_START = 7;
    public int MIDDAY_START = 11;
    public int EVENING_START = 16;
    public int NIGHT_START = 21;

    public EnumDisplayTime startTime = EnumDisplayTime.SUNRISE;

    [Header("Length of a day in real life seconds.")]
    public float DayLength = 20.0f;

    //How far into the cycle the time is.
    public float TimeAlpha { get; private set; } = 0.0f;

    public EnumTime CurrentTimeOfDay { get; private set; }
    public EnumDisplayTime ExternalTimeOfDay { get; private set; }

    private void Start()
    {
        InitTimeOfDay();
        GlobalTime.SetDayLength(DayLength);

        if (SwitchTimesList.TryGetValue(startTime, out float ticks))
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
        SwitchTimesList.Clear();
        SwitchTimesList.Add(EnumDisplayTime.SUNRISE, SUNRISE_START * SigiTime.SECONDS * SigiTime.MINUTES);
        SwitchTimesList.Add(EnumDisplayTime.MORNING, MORNING_START * SigiTime.SECONDS * SigiTime.MINUTES);
        SwitchTimesList.Add(EnumDisplayTime.MIDDAY, MIDDAY_START * SigiTime.SECONDS * SigiTime.MINUTES);
        SwitchTimesList.Add(EnumDisplayTime.EVENING, EVENING_START * SigiTime.SECONDS * SigiTime.MINUTES);
        SwitchTimesList.Add(EnumDisplayTime.NIGHT, NIGHT_START * SigiTime.SECONDS * SigiTime.MINUTES);
    }

    private void CalculateDisplayTime()
    {
        UpdateTimeOfDay();

        EnumDisplayTime NextTimeOfDay = (EnumDisplayTime)(((int)ExternalTimeOfDay + 1) % (int)EnumDisplayTime.SIZE);
        SwitchTimesList.TryGetValue(ExternalTimeOfDay, out float startTime);
        SwitchTimesList.TryGetValue(NextTimeOfDay, out float endTime);
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

    private void UpdateTimeOfDay()
    {
        if (GlobalTime.Hour < SUNRISE_START || GlobalTime.Hour >= NIGHT_START)
        {
            ExternalTimeOfDay = EnumDisplayTime.NIGHT;
        }
        else if (GlobalTime.Hour < MORNING_START)
        {
            ExternalTimeOfDay = EnumDisplayTime.SUNRISE;
        }
        else if (GlobalTime.Hour < MIDDAY_START)
        {
            ExternalTimeOfDay = EnumDisplayTime.MORNING;
        }
        else if (GlobalTime.Hour < EVENING_START)
        {
            ExternalTimeOfDay = EnumDisplayTime.MIDDAY;
        }
        else
        {
            ExternalTimeOfDay = EnumDisplayTime.EVENING;
        }

        CurrentTimeOfDay = ToEnumTime(ExternalTimeOfDay);
    }

    private EnumTime ToEnumTime(EnumDisplayTime displayTime)
    {
        switch (displayTime)
        {
            case EnumDisplayTime.SUNRISE:
            case EnumDisplayTime.MORNING:
                return EnumTime.MORNING;
            case EnumDisplayTime.MIDDAY:
                return EnumTime.MIDDAY;
            case EnumDisplayTime.EVENING:
            case EnumDisplayTime.NIGHT:
            default:
                return EnumTime.NIGHT;
        }
    }

    public class SigiTime
    {
        public int Day { get; private set; }
        public int Hour { get; private set; }
        public int Minute { get; private set; }
        public int Second { get; private set; }

        private float multiplier;

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
            totalTimeCount += Time.deltaTime * multiplier;
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

        public void SetDayLength(float realLifeSeconds)
        {
            multiplier = TICKS / realLifeSeconds;
        }
    }

    //Custom values to map to EnumTime
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
    MIDDAY = 2,
    NIGHT = 4,
    SIZE = 3
}