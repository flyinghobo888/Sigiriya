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

    public EnumDisplayTime NextTimeOfDay { get; private set; }

    private IEnumerator DayCoroutine = null;

	//TODO: dirty
	public bool timePaused = false;

    private void Start()
    {
        InitTimeOfDay();
        GlobalTime.SetDayLength(DayLength);

        //if (SwitchTimesList.TryGetValue(startTime, out float ticks))
        //{
        //    GlobalTime.SetTickValue(ticks);
        //}

        //TODO: Call StartDay() when we want to start a new day from somewhere else.
        StartDay(false);
    }

    public void StartDay(bool incrementDay)
    {
        if (IsDayOver())
        {
            Debug.Log("INCREMENTING IS: " + incrementDay);
            GlobalTime.SetTimeOfDay(SUNRISE_START, incrementDay);
            UpdateTimeOfDay();

            DayCoroutine = GoThroughDay();
            StartCoroutine(DayCoroutine);
        }
        else
        {
            Debug.Log("Cannot start a new day while it is still day time.");
        }
    }

    public bool IsDayOver()
    {
        return (DayCoroutine == null);
    }

    private IEnumerator GoThroughDay()
    {
        Debug.Log("STARTING DAY");
        Debug.Log("Time: " + GlobalTime.Ticks + " | " + 
            GlobalTime.Day + "D " + GlobalTime.Hour + "H " + GlobalTime.Minute + "M " + GlobalTime.Second + "S " +
            " | TOD: " + ExternalTimeOfDay);

        EventAnnouncer.OnDayIsStarting?.Invoke();

        while (ExternalTimeOfDay != EnumDisplayTime.NIGHT)
        {
			if (!timePaused)
			{
				GlobalTime.Tick();
			}
            CalculateDisplayTime();
            yield return null;
        }

        //END OF DAY
        Debug.Log("END OF DAY");
        DayCoroutine = null;
        EventAnnouncer.OnDayIsEnding?.Invoke();
    }

    private void InitTimeOfDay()
    {
        UpdateTimeOfDay();

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

        if (ExternalTimeOfDay == NextTimeOfDay)
        {
            EventAnnouncer.OnTimeOfDayChanged?.Invoke(ExternalTimeOfDay);
        }

        NextTimeOfDay = (EnumDisplayTime)(((int)ExternalTimeOfDay + 1) % (int)EnumDisplayTime.SIZE);
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

        EventAnnouncer.OnTimeTickUpdated?.Invoke(GlobalTime);
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

public enum EnumTime
{
    MORNING = 1,
    MIDDAY = 2,
    NIGHT = 4,
    SIZE = 3
}

public class SigiTime
{
    public int Day { get; private set; }
    public int Hour { get; private set; }
    public int Minute { get; private set; }
    public int Second { get; private set; }

    private static float multiplier;

    private float totalTimeCount = 0.0f;
    private float tickCounter = 0.0f;
    public float Ticks = 0.0f;

    public const int HOURS = 24;
    public const int MINUTES = 60;
    public const int SECONDS = 60;
    public const float TICKS = HOURS * MINUTES * SECONDS;

    public SigiTime(int d = 0, int h = 0, int m = 0, int s = 0)
    {
		Day = d;
        Hour = h;
        Minute = m;
        Second = s;
        totalTimeCount = (Day * TICKS) + (Hour * MINUTES * SECONDS) + (Minute * SECONDS) + Second;
    }

    public void Tick()
    {
        totalTimeCount += Time.deltaTime * multiplier;

        UpdateVars();

        //Debug.Log("Time: " + Day + "d " + Hour + "h " + Minute + "m " + Second + "s");
    }

    public bool CountDown()
    {
        totalTimeCount -= Time.deltaTime * multiplier;

        if (totalTimeCount <= 0)
        {
            totalTimeCount = 0;
            Ticks = 0;
            tickCounter = 0;
            Second = 0;
            Minute = 0;
            Hour = 0;
            Day = 0;
			return true;
        }

        UpdateVars();

		return false;
    }

    private void UpdateVars()
    {
        Ticks = totalTimeCount % TICKS;
        tickCounter = totalTimeCount;

        Second = (int)Ticks;
        Second %= SECONDS;

        Minute = (int)(tickCounter /= SECONDS);
        Minute %= MINUTES;

        Hour = (int)(tickCounter /= MINUTES);
        Hour %= HOURS;

        Day = (int)(tickCounter /= HOURS);
    }

    //public void SetTickValue(float tickValue)
    //{
    //    totalTimeCount = tickValue;
    //}

    public void SetDayLength(float realLifeSeconds)
    {
        multiplier = TICKS / realLifeSeconds;
    }

    public void SetTimeOfDay(int hour, bool incrementDay)
    {
        if (incrementDay)
        {
            totalTimeCount = Day * TICKS + TICKS;
        }

        totalTimeCount += (hour * MINUTES * SECONDS);

        UpdateVars();
    }

	public void IncrementTime(SigiTime time)
	{
		totalTimeCount += time.totalTimeCount;
	}
}
