﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
//The Global Time Tracker is responsible for tracking the time through out the game.
public class GlobalTimeTracker : ManagerBase<GlobalTimeTracker>
{
    //For now, just change the time every 5 seconds
    private float currentTime;
    private EnumTime currentTimeOfDay;

    [SerializeField] private SpriteRenderer background;

    [SerializeField] private Material sunriseOverlay;
    [SerializeField] private Material morningOverlay;
    [SerializeField] private Material middayOverlay;
    [SerializeField] private Material eveningOverlay;
    [SerializeField] private Material nightOverlay;

    private List<Material> backgroundOverlays = new List<Material> ();
    private int currentIndex = 0;



    private Color currentTopColor;
    private Color targetTopColor;

    private Color currentBottomColor;
    private Color targetBottomColor;



    private void Start()
    {
        currentTimeOfDay = EnumTime.MORNING;

        backgroundOverlays.Clear ();
        backgroundOverlays.Add (sunriseOverlay);
        backgroundOverlays.Add (morningOverlay);
        backgroundOverlays.Add (middayOverlay);
        backgroundOverlays.Add (eveningOverlay);
        backgroundOverlays.Add (nightOverlay);
        SetNewColors ();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        background.material.SetColor ("Color_22E35091", Color.Lerp (currentTopColor, targetTopColor, currentTime/1));
        background.material.SetColor ("Color_B0472F4B", Color.Lerp (currentBottomColor, targetBottomColor, currentTime/1));

        if (currentTime >= 1.0f)
        {
            currentIndex = (((int) currentTimeOfDay + 1) % (int) EnumTime.SIZE);
            currentTime = 0.0f;
            currentTimeOfDay = (EnumTime) (currentIndex);
            //background.material = backgroundOverlays [currentIndex];
            SetNewColors ();
        }
    }

    void SetNewColors()
    {
        Material currentMat = backgroundOverlays [currentIndex];
        Material nextMat = backgroundOverlays [(((int) currentTimeOfDay + 1) % (int) EnumTime.SIZE)];

        currentTopColor = currentMat.GetColor ("Color_22E35091");
        currentBottomColor = currentMat.GetColor ("Color_B0472F4B");

        targetTopColor = nextMat.GetColor ("Color_22E35091");
        targetBottomColor = nextMat.GetColor ("Color_B0472F4B");
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
