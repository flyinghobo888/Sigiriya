using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//The Global Time Tracker is responsible for tracking the time through out the game.
public class GlobalTimeTracker : ManagerBase<GlobalTimeTracker>
{
    //For now, just change the time every few seconds
    private float totalTime = 2.0f;
    public float TotalTimeInverse { get; private set; }
    public float CurrentTime { get; private set; }

    //How far into the cycle the time is.
    public float TimeAlpha { get; private set; }

    public EnumTime CurrentTimeOfDay { get; private set; }

    [SerializeField] private Image background = null;

    private int currentIndex = 0;

    private Color currentTopColor;
    private Color targetTopColor;

    private Color currentBottomColor;
    private Color targetBottomColor;

    private void Start()
    {
        TotalTimeInverse = 1.0f / totalTime;
        CurrentTimeOfDay = EnumTime.MORNING;

        //SetNewColors();
    }

    private void Update()
    {
        CurrentTime += Time.deltaTime;
        TimeAlpha = CurrentTime * TotalTimeInverse;

        //background.material.SetColor("Color_22E35091", Color.Lerp(currentTopColor, targetTopColor, currentTime * totalTimeInverse));
        //background.material.SetColor("Color_B0472F4B", Color.Lerp(currentBottomColor, targetBottomColor, currentTime * totalTimeInverse));

        if (CurrentTime >= totalTime)
        {
            currentIndex = (((int)CurrentTimeOfDay + 1) % (int)EnumTime.SIZE);
            CurrentTime = 0.0f;
            CurrentTimeOfDay = (EnumTime)(currentIndex);
            TimeAlpha = 0.0f;
            EventAnnouncer.OnTimeChanged(CurrentTimeOfDay);
            //SetNewColors();
        }
    }

    //void SetNewColors()
    //{
        //Material currentMat = backgroundOverlays[currentIndex];
        //Material nextMat = backgroundOverlays[(((int)currentTimeOfDay + 1) % (int)EnumTime.SIZE)];

        //currentTopColor = currentMat.GetColor("Color_22E35091");
        //currentBottomColor = currentMat.GetColor("Color_B0472F4B");

        //targetTopColor = nextMat.GetColor("Color_22E35091");
        //targetBottomColor = nextMat.GetColor("Color_B0472F4B");
    //}
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
