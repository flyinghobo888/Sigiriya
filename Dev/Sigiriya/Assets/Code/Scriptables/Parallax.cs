using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Background", menuName = "Background")]
public class Parallax : ScriptableObject
{
    public EnumLocationModifier locationModifier = EnumLocationModifier.NORMAL;

    public Sprite Background = null;
    public Sprite Midground = null;
    public Sprite Foreground = null;

    [Header("The speed in relation to drag speed")]
    [Range(0.1f, 3.0f)] public float BackgroundSpeedMult = 0.61f;
    [Range(0.1f, 3.0f)] public float MidgroundSpeedMult = 1.0f;
    [Range(0.1f, 3.0f)] public float ForegroundSpeedMult = 1.43f;
}

//Modifiers that might change how the location looks.
//Should use the flag bank maybe. Gotta talk to Karim about that
public enum EnumLocationModifier : int
{
    NORMAL
}