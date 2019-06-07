﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
//Responsible for storing general things that many things in the game can use.
//Do not start putting a ton of stuff in here! Only use this for when multiple systems need to share things.
public class GameMaster : ManagerBase<GameMaster>
{
    private static Fade screenFade;

    private void Awake()
    {
        GetFade();
    }

    public Fade GetFade()
    {
        if (!screenFade)
        {
            Debug.Log("HELLO");
            screenFade = Instantiate(Resources.Load<GameObject>("Unity/Prefabs/_FADE")).GetComponent<Fade>();
            DontDestroyOnLoad(screenFade);
        }

        return screenFade;
    }
}
