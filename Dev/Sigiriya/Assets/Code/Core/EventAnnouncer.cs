using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Authors: Andrew Rimpici
//Designed to hold all of the possible events for the game.
public class EventAnnouncer : ManagerBase<EventAnnouncer>
{
    /* Scene Events */

    //Sends an event to change to the target scene (Changes the scene to the target scene).
    public delegate void RequestSceneChange(EnumScene scene, bool shouldFade);
    public static RequestSceneChange OnRequestSceneChange;
}
